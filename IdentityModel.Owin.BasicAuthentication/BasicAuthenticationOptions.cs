﻿/*
 * Copyright 2014, 2015 Dominick Baier, Brock Allen
 * (contributed by Pedro Felix)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.Owin.Security;

namespace IdentityModel.Owin.BasicAuthentication
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationMiddleware.CredentialValidationFunction CredentialValidationFunction { get; private set; }
        public string Realm { get; private set; }
        public const string BasicAuthenticationType = "Basic";

        public BasicAuthenticationOptions(string realm, BasicAuthenticationMiddleware.CredentialValidationFunction validationFunction)
                : base(BasicAuthenticationType)
        {
            Realm = realm;
            CredentialValidationFunction = validationFunction;
        }
    }
}