﻿// <copyright file="Response.cs" company="Twilio SendGrid">
// Copyright (c) Twilio SendGrid. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SendGrid
{
    /// <summary>
    /// The response received from an API call to Twilio SendGrid.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// The status code returned from Twilio SendGrid.
        /// </summary>
        private HttpStatusCode _statusCode;

        /// <summary>
        /// The response body returned from Twilio SendGrid.
        /// </summary>
        private HttpContent _body;

        /// <summary>
        /// The response headers returned from Twilio SendGrid.
        /// </summary>
        private HttpResponseHeaders _headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="statusCode">https://docs.microsoft.com/dotnet/api/system.net.httpstatuscode.</param>
        /// <param name="responseBody">https://docs.microsoft.com/dotnet/api/system.net.http.httpcontent.</param>
        /// <param name="responseHeaders">https://docs.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders.</param>
        public Response(HttpStatusCode statusCode, HttpContent responseBody, HttpResponseHeaders responseHeaders)
        {
            this.StatusCode = statusCode;
            this.Body = responseBody;
            this.Headers = responseHeaders;
        }

        /// <summary>
        /// Gets or sets the status code returned from Twilio SendGrid.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this._statusCode;
            }

            set
            {
                this._statusCode = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether Status Code of this response indicates success.
        /// </summary>
        public bool IsSuccessStatusCode
        {
            get { return ((int)StatusCode >= 200) && ((int)StatusCode <= 299); }
        }

        /// <summary>
        /// Gets or sets the response body returned from Twilio SendGrid.
        /// <see href="https://docs.microsoft.com/dotnet/api/system.net.http.httpcontent"></see>
        /// </summary>
        public HttpContent Body
        {
            get
            {
                return this._body;
            }

            set
            {
                this._body = value;
            }
        }

        /// <summary>
        /// Gets or sets the response headers returned from Twilio SendGrid.
        /// <see href="https://docs.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders"></see>
        /// </summary>
        public HttpResponseHeaders Headers
        {
            get
            {
                return this._headers;
            }

            set
            {
                this._headers = value;
            }
        }

        /// <summary>
        /// Converts string formatted response body to a Dictionary.
        /// </summary>
        /// <returns>Dictionary object representation of HttpContent.</returns>
        public virtual async Task<Dictionary<string, dynamic>> DeserializeResponseBodyAsync()
        {
            if (this._body is null)
            {
                return new Dictionary<string, dynamic>();
            }

            var stringContent = await this._body.ReadAsStringAsync().ConfigureAwait(false);
            var dsContent = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(stringContent);
            return dsContent;
        }

        /// <summary>
        /// Converts string formatted response headers to a Dictionary.
        /// </summary>
        /// <returns>Dictionary object representation of HttpResponseHeaders.</returns>
        public virtual Dictionary<string, string> DeserializeResponseHeaders()
        {
            var dsContent = new Dictionary<string, string>();
            if (this._headers == null)
            {
                return dsContent;
            }

            foreach (var pair in this._headers)
            {
                dsContent.Add(pair.Key, pair.Value.First());
            }

            return dsContent;
        }
    }
}
