﻿namespace Disqord.Http
{
    public enum HttpResponseStatusCode
    {
        Ok = 200,

        Created = 201,

        NoContent = 204,

        NotModified = 304,

        BadRequest = 400,

        Unauthorized = 401,

        Forbidden = 403,

        NotFound = 404,

        MethodNotAllowed = 405,

        TooManyRequests = 429,

        InternalServerError = 500,

        GatewayUnavailable = 502
    }
}
