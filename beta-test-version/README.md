# C# .Net Core XML Web Services Example

This directory contains example code demonstrating how to use the XML-RPC 
Web Services interface to automate and manage the PaperCut MF/NG system, accounts and users.

`ServerCommandProxy.cs` : This C# class wraps the XML-RPC Web Services network calls
exposing them as C# methods.

`Program.cs`: An example on how the ServerCommandProxy class is used.

To compile the example with the Microsoft .NET Core SDK use the following guidelines:

(Note these examples assume PowerShell on Windows, they need to be adapted for other platforms)

1. Make sure the .NET Core SDK is correctly installed

2. Create a project directory

3. Open a command prompt and "cd" to this directory.

4. Create a new .NET Core console project

        dotnet new console

4. Install an XML-RPC library. We are using Kveer.XmlRPC which uses the CookComputing.XmlRpc namespace
(see .NET Foundation notes below)

        dotnet add package Kveer.XmlRPC --version 1.1.1

8. Copy the files `Program.cs` ad `ServerCommnadProxy.cs` into the current project directory

9. Set up a secure token by editing the PaperCut MF/NG advanced config key `auth.webservices.auth-token`.
For example (note this example must have no additional line breaks inserted after `--%`):

        & 'C:\Program Files\PaperCut MF\server\bin\win\server-command.exe' set-config auth.webservices.auth-token
        --% "{""csharp_client"":""Zuj0hiazoo5hahwa"",""default"":""token""}"

10. Edit the `Program.cs` file so the correct security token (`Zuj0hiazoo5hahwa` in the above example) is used.

    NOTE: In versions of PaperCut MF/NG prior to 19.2.3 the PaperCut namespace was not used.
    So for instance

        _serverProxy = new PaperCut.ServerCommandProxy(server, 9191, authToken);

    should, in older releases, be

            _serverProxy = new ServerCommandProxy(server, 9191, authToken);

11. Compile and run using the command

        dotnet run

For more information on using the .NET Core command line tool and deployment options see
https://docs.microsoft.com/en-us/dotnet/core/tutorials/cli-create-console-app

## Running the sample code on .NET Foundation.

The same example code can be build and run on the lagacy .NET Foundation framework using the original
Cook Computing XML RPC library (see https://www.nuget.org/packages/xmlrpcnet/). Note however this library version is
no longer maintained.

Because the Kveer.XmlRPC library is a port of the original Cook library no code changes are required. The PaperCut Software
supplied `ServerCommandProxy.cs` code should work with both libraries.
