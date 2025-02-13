﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.ResourceManager.Core
{
    internal partial class TagRestOperations
    {
        private string subscriptionId;
        private Uri endpoint;
        private ClientDiagnostics _clientDiagnostics;
        private HttpPipeline _pipeline;

        /// <summary> Initializes a new instance of TagsRestOperations. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="subscriptionId"> The ID of the target subscription. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/> is null. </exception>
        public TagRestOperations(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, string subscriptionId, Uri endpoint = null)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            endpoint ??= new Uri("https://management.azure.com");

            this.subscriptionId = subscriptionId;
            this.endpoint = endpoint;
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        internal Azure.Core.HttpMessage CreateDeleteValueRequest(string tagName, string tagValue)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/tagNames/", false);
            uri.AppendPath(tagName, true);
            uri.AppendPath("/tagValues/", false);
            uri.AppendPath(tagValue, true);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> This operation allows deleting a value from the list of predefined values for an existing predefined tag name. The value being deleted must not be in use as a tag value for the given tag name for any resource. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to delete. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or <paramref name="tagValue"/> is null. </exception>
        public async Task<Response> DeleteValueAsync(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }
            if (tagValue == null)
            {
                throw new ArgumentNullException(nameof(tagValue));
            }

            using var message = CreateDeleteValueRequest(tagName, tagValue);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation allows deleting a value from the list of predefined values for an existing predefined tag name. The value being deleted must not be in use as a tag value for the given tag name for any resource. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to delete. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or <paramref name="tagValue"/> is null. </exception>
        public Response DeleteValue(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }
            if (tagValue == null)
            {
                throw new ArgumentNullException(nameof(tagValue));
            }

            using var message = CreateDeleteValueRequest(tagName, tagValue);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateCreateOrUpdateValueRequest(string tagName, string tagValue)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Put;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/tagNames/", false);
            uri.AppendPath(tagName, true);
            uri.AppendPath("/tagValues/", false);
            uri.AppendPath(tagValue, true);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> This operation allows adding a value to the list of predefined values for an existing predefined tag name. A tag value can have a maximum of 256 characters. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to create. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or <paramref name="tagValue"/> is null. </exception>
        public async Task<Response<PreDefinedTagValue>> CreateOrUpdateValueAsync(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }
            if (tagValue == null)
            {
                throw new ArgumentNullException(nameof(tagValue));
            }

            using var message = CreateCreateOrUpdateValueRequest(tagName, tagValue);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 201:
                    {
                        PreDefinedTagValue value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = PreDefinedTagValue.DeserializeTagValue(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation allows adding a value to the list of predefined values for an existing predefined tag name. A tag value can have a maximum of 256 characters. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="tagValue"> The value of the tag to create. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or <paramref name="tagValue"/> is null. </exception>
        public Response<PreDefinedTagValue> CreateOrUpdateValue(string tagName, string tagValue, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }
            if (tagValue == null)
            {
                throw new ArgumentNullException(nameof(tagValue));
            }

            using var message = CreateCreateOrUpdateValueRequest(tagName, tagValue);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 201:
                    {
                        PreDefinedTagValue value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = PreDefinedTagValue.DeserializeTagValue(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateCreateOrUpdateRequest(string tagName)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Put;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/tagNames/", false);
            uri.AppendPath(tagName, true);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> This operation allows adding a name to the list of predefined tag names for the given subscription. A tag name can have a maximum of 512 characters and is case-insensitive. Tag names cannot have the following prefixes which are reserved for Azure use: &apos;microsoft&apos;, &apos;azure&apos;, &apos;windows&apos;. </summary>
        /// <param name="tagName"> The name of the tag to create. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is null. </exception>
        public async Task<Response<PreDefinedTagData>> CreateOrUpdateAsync(string tagName, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }

            using var message = CreateCreateOrUpdateRequest(tagName);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 201:
                    {
                        PreDefinedTagData value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = PreDefinedTagData.DeserializeTagDetails(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation allows adding a name to the list of predefined tag names for the given subscription. A tag name can have a maximum of 512 characters and is case-insensitive. Tag names cannot have the following prefixes which are reserved for Azure use: &apos;microsoft&apos;, &apos;azure&apos;, &apos;windows&apos;. </summary>
        /// <param name="tagName"> The name of the tag to create. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is null. </exception>
        public Response<PreDefinedTagData> CreateOrUpdate(string tagName, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }

            using var message = CreateCreateOrUpdateRequest(tagName);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 201:
                    {
                        PreDefinedTagData value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = PreDefinedTagData.DeserializeTagDetails(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateDeleteRequest(string tagName)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/tagNames/", false);
            uri.AppendPath(tagName, true);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> This operation allows deleting a name from the list of predefined tag names for the given subscription. The name being deleted must not be in use as a tag name for any resource. All predefined values for the given name must have already been deleted. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is null. </exception>
        public async Task<Response> DeleteAsync(string tagName, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }

            using var message = CreateDeleteRequest(tagName);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation allows deleting a name from the list of predefined tag names for the given subscription. The name being deleted must not be in use as a tag name for any resource. All predefined values for the given name must have already been deleted. </summary>
        /// <param name="tagName"> The name of the tag. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is null. </exception>
        public Response Delete(string tagName, CancellationToken cancellationToken = default)
        {
            if (tagName == null)
            {
                throw new ArgumentNullException(nameof(tagName));
            }

            using var message = CreateDeleteRequest(tagName);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateListRequest()
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/tagNames", false);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> This operation performs a union of predefined tags, resource tags, resource group tags and subscription tags, and returns a summary of usage for each tag name and value under the given subscription. In case of a large number of tags, this operation may return a previously cached result. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<PreDefinedTagsListResult>> ListAsync(CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest();
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        PreDefinedTagsListResult value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = PreDefinedTagsListResult.DeserializeTagsListResult(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation performs a union of predefined tags, resource tags, resource group tags and subscription tags, and returns a summary of usage for each tag name and value under the given subscription. In case of a large number of tags, this operation may return a previously cached result. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<PreDefinedTagsListResult> List(CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest();
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        PreDefinedTagsListResult value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = PreDefinedTagsListResult.DeserializeTagsListResult(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateListNextPageRequest(string nextLink)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendRawNextLink(nextLink, false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> This operation performs a union of predefined tags, resource tags, resource group tags and subscription tags, and returns a summary of usage for each tag name and value under the given subscription. In case of a large number of tags, this operation may return a previously cached result. </summary>
        /// <param name="nextLink"> The URL to the next page of results. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="nextLink"/> is null. </exception>
        public async Task<Response<PreDefinedTagsListResult>> ListNextPageAsync(string nextLink, CancellationToken cancellationToken = default)
        {
            if (nextLink == null)
            {
                throw new ArgumentNullException(nameof(nextLink));
            }

            using var message = CreateListNextPageRequest(nextLink);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        PreDefinedTagsListResult value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = PreDefinedTagsListResult.DeserializeTagsListResult(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation performs a union of predefined tags, resource tags, resource group tags and subscription tags, and returns a summary of usage for each tag name and value under the given subscription. In case of a large number of tags, this operation may return a previously cached result. </summary>
        /// <param name="nextLink"> The URL to the next page of results. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="nextLink"/> is null. </exception>
        public Response<PreDefinedTagsListResult> ListNextPage(string nextLink, CancellationToken cancellationToken = default)
        {
            if (nextLink == null)
            {
                throw new ArgumentNullException(nameof(nextLink));
            }

            using var message = CreateListNextPageRequest(nextLink);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        PreDefinedTagsListResult value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = PreDefinedTagsListResult.DeserializeTagsListResult(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateCreateOrUpdateAtScopeRequest(string scope, TagResourceData parameters)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Put;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/", false);
            uri.AppendPath(scope, false);
            uri.AppendPath("/providers/Microsoft.Resources/tags/default", false);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Content-Type", "application/json");
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(parameters);
            request.Content = content;
            return message;
        }

        /// <summary> This operation allows adding or replacing the entire set of tags on the specified resource or subscription. The specified entity can have a maximum of 50 tags. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="parameters"> The TagsResource to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> or <paramref name="parameters"/> is null. </exception>
        public async Task<Response<TagResourceData>> CreateOrUpdateAtScopeAsync(string scope, TagResourceData parameters, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var message = CreateCreateOrUpdateAtScopeRequest(scope, parameters);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        TagResourceData value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = TagResourceData.DeserializeTagsResource(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation allows adding or replacing the entire set of tags on the specified resource or subscription. The specified entity can have a maximum of 50 tags. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="parameters"> The TagsResource to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> or <paramref name="parameters"/> is null. </exception>
        public Response<TagResourceData> CreateOrUpdateAtScope(string scope, TagResourceData parameters, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var message = CreateCreateOrUpdateAtScopeRequest(scope, parameters);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        TagResourceData value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = TagResourceData.DeserializeTagsResource(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateUpdateAtScopeRequest(string scope, TagPatchResource parameters)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Patch;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/", false);
            uri.AppendPath(scope, false);
            uri.AppendPath("/providers/Microsoft.Resources/tags/default", false);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Content-Type", "application/json");
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(parameters);
            request.Content = content;
            return message;
        }

        /// <summary> This operation allows replacing, merging or selectively deleting tags on the specified resource or subscription. The specified entity can have a maximum of 50 tags at the end of the operation. The &apos;replace&apos; option replaces the entire set of existing tags with a new set. The &apos;merge&apos; option allows adding tags with new names and updating the values of tags with existing names. The &apos;delete&apos; option allows selectively deleting tags based on given names or name/value pairs. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="parameters"> The TagsPatchResource to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> or <paramref name="parameters"/> is null. </exception>
        public async Task<Response<TagResourceData>> UpdateAtScopeAsync(string scope, TagPatchResource parameters, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var message = CreateUpdateAtScopeRequest(scope, parameters);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        TagResourceData value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = TagResourceData.DeserializeTagsResource(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> This operation allows replacing, merging or selectively deleting tags on the specified resource or subscription. The specified entity can have a maximum of 50 tags at the end of the operation. The &apos;replace&apos; option replaces the entire set of existing tags with a new set. The &apos;merge&apos; option allows adding tags with new names and updating the values of tags with existing names. The &apos;delete&apos; option allows selectively deleting tags based on given names or name/value pairs. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="parameters"> The TagsPatchResource to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> or <paramref name="parameters"/> is null. </exception>
        public Response<TagResourceData> UpdateAtScope(string scope, TagPatchResource parameters, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var message = CreateUpdateAtScopeRequest(scope, parameters);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        TagResourceData value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = TagResourceData.DeserializeTagsResource(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateGetAtScopeRequest(string scope)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/", false);
            uri.AppendPath(scope, false);
            uri.AppendPath("/providers/Microsoft.Resources/tags/default", false);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> Gets the entire set of tags on a resource or subscription. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> is null. </exception>
        public async Task<Response<TagResourceData>> GetAtScopeAsync(string scope, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            using var message = CreateGetAtScopeRequest(scope);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        TagResourceData value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = TagResourceData.DeserializeTagsResource(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Gets the entire set of tags on a resource or subscription. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> is null. </exception>
        public Response<TagResourceData> GetAtScope(string scope, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            using var message = CreateGetAtScopeRequest(scope);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        TagResourceData value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = TagResourceData.DeserializeTagsResource(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal Azure.Core.HttpMessage CreateDeleteAtScopeRequest(string scope)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/", false);
            uri.AppendPath(scope, false);
            uri.AppendPath("/providers/Microsoft.Resources/tags/default", false);
            uri.AppendQuery("api-version", "2019-10-01", true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            return message;
        }

        /// <summary> Deletes the entire set of tags on a resource or subscription. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> is null. </exception>
        public async Task<Response> DeleteAtScopeAsync(string scope, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            using var message = CreateDeleteAtScopeRequest(scope);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Deletes the entire set of tags on a resource or subscription. </summary>
        /// <param name="scope"> The resource scope. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="scope"/> is null. </exception>
        public Response DeleteAtScope(string scope, CancellationToken cancellationToken = default)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            using var message = CreateDeleteAtScopeRequest(scope);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }
    }
}
