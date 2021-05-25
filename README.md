# Contoso Conference

[![Build Status](https://dev.azure.com/feliperomeromx/Projects/_apis/build/status/feliperomero3.ContosoConference?branchName=master)](https://dev.azure.com/feliperomeromx/Projects/_build/latest?definitionId=12&branchName=master)

ASP.NET Core 3.1 Web API to manage the *fictitious* world-famous Contoso Conference

## Overview

Contoso Conference is held every other year and it presents speakers from all around the world to talk about the latest technologies

## Prerequisites

- An Azure account with an active subscription
- An Azure AD tenant
- An Azure AD Application to represent the Web API
- An Azure AD Application to represent the client (e. g. Postman)
- Visual Studio 2019 16.6 (with Web development workload)

## Getting started

1. Clone this project.
2. Replace `AzureAd` configuration section in `appsettings.json` file with the values generated for your Azure AD registered App that represents your Web API.

   ```text
   "AzureAd": {
     "Instance": "https://login.microsoftonline.com/",
     "Domain": "[Enter the domain of your tenant, e.g. contoso.onmicrosoft.com]",
     "ClientId": "[Enter the Client Id (obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403]",
     "TenantId": "[Enter the Tenant Id (obtained from the Azure portal), e.g. da41245a5-11b3-996c-00a8-4d99re19f292]",
     "Audience": "Enter the Application ID URI (obtained from the Azure portal), e.g. api://ba74781c2-53c2-442a-97c2-3d60re42f403"
   }
   ...
   ```

3. Build the solution file.
4. Press F5 to start the debug session.

## Notes

This is my attempt at building a REST Web API on ASP.NET Core protected with Azure Active Directory using Microsoft Identity Platform.

## License

[MIT License](./LICENSE)

Copyright (c) 2020 Felipe Romero
