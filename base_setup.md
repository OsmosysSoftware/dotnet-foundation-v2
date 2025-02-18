# Base Setup for .NET Foundation Repository

This document provides step-by-step instructions to set up the base structure for your .NET projects, including an API project and a Core library.

## Prerequisites
Ensure you have the following installed:
- [.NET SDK 8.0.11](https://dotnet.microsoft.com/en-us/download)
- Git
- A terminal (Command Prompt, PowerShell, or Linux/macOS shell)

## 1. Initialize the Root Directory
```sh
mkdir DotNetFoundationV2 && cd DotNetFoundationV2
```

## 2. Create the `src` Directory
```sh
mkdir src && cd src
```

## 3. Create the API Project
```sh
dotnet new webapi -o Api
```
This will create an ASP.NET Core Web API inside `src/Api`.

## 4. Create the Core Library
```sh
dotnet new classlib -o Core
```
This will create a .NET Class Library inside `src/Core`.

## 5. Add `Core` as a Reference to `Api`
```sh
cd Api
dotnet add reference ../Core/Core.csproj
cd ..
```

## 6. Create a Solution File and Add Projects
```sh
dotnet new sln -n DotNetFoundation
```
Add projects to the solution:
```sh
dotnet sln DotNetFoundation.sln add Api/Api.csproj
```
```sh
dotnet sln DotNetFoundation.sln add Core/Core.csproj
```

## 7. Verify the Directory Structure
Run the following command:
```sh
tree
```
Expected output:
```
src/
├── Api/
│   ├── Api.csproj
│   ├── Program.cs
│   └── ...
└── Core/
    ├── Core.csproj
    └── ...
```


This setup provides a structured foundation for your .NET projects. 🚀

