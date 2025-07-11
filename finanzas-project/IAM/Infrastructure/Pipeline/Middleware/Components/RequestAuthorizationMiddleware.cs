﻿using finanzas_project.IAM.Application.Internal.OutboundServices;
using finanzas_project.IAM.Domain.Model.Queries;
using finanzas_project.IAM.Domain.Services;
using finanzas_project.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace finanzas_project.IAM.Infrastructure.Pipeline.Middleware.Components
{
    public class RequestAuthorizationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, IUserQueryService userQueryService, ITokenService tokenService)
        {
            Console.WriteLine("Entering InvokeAsync");
            // skip authorization if endpoint is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.Request.HttpContext.GetEndpoint()!
                .Metadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute));
            Console.WriteLine($"Allow Anonymous is {allowAnonymous}");
            if (allowAnonymous)
            {
                Console.WriteLine("Skipping authorization");
                // [AllowAnonymous] attribute is set, so skip authorization
                await next(context);
                return;
            }
            Console.WriteLine("Entering authorization");
            // Get token from request header
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            // If token is null then throw exception
            if (token is null) throw new Exception("Null or invalid token");

            // Validate token
            var userId = await tokenService.ValidateToken(token);

            // If token is invalid then the userId will be null, so and exception must be thrown
            if (userId is null) throw new Exception("Invalid token");

            // Create a GetUserByIdQuery object
            var getUserByIdQuery = new GetUserByIdQuery(userId.Value);

            // Get the user by id through the userQueryService
            var user = await userQueryService.Handle(getUserByIdQuery);

            // Set the user in HTTP Context
            Console.WriteLine("Successful authorization. Updating Context...");
            context.Items["User"] = user;

            // Continue with the request pipeline
            await next(context);

        }
    }
}
