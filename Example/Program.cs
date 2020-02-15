/*
 * (c) Copyright 1999-2011 PaperCut Software International Pty Ltd.
 *
 * An C# .NET Example demonstrating how to script Application Server commands using
 * the XML-RPC Web Services API's.  See documentation and the ServerCommandProxy class
 * for all available commands.
 */
using System;
using CookComputing.XmlRpc; // So we can process XmlRpcFaultException
using PaperCut;
public class Example {

    private static PaperCut.ServerCommandProxy _serverProxy;

    public static void Main() {

        // This should be the value defined in the advanced config key auth.webservices.auth-token. Change as appropriate.
        const string authToken = "token";
        const string user      = "test-user-account";
        const string account   = "test-shared-account";
        const string server    = "localhost";

        // Create an instance of the server command proxy class.
        _serverProxy = new PaperCut.ServerCommandProxy(server, 9191, authToken);

        UserExample(user);
        SharedAccountExample(user, account);
    }

    private static void UserExample(string user) {
        //
        // USER EXAMPLE
        //
        Console.WriteLine();
        Console.WriteLine("----------------");
        Console.WriteLine("- User Example -");
        Console.WriteLine("----------------");
        Console.WriteLine();
        try {
            if (_serverProxy.UserExists(user) && _serverProxy.IsUserExists(user)) {
                Console.WriteLine($"The test user account \"{user}\" already exists.");
            } else {
                // user does not exist so create
                Console.WriteLine($"Creating user: {user}");
                _serverProxy.AddNewUser(user);
            }
            // record the current balance
            double currentBalance = Double.Parse(_serverProxy.GetUserProperty(user, "balance"));
            // give the user extra credit of $100.00 just for fun!
            _serverProxy.AdjustUserAccountBalance(user, 100.00, "This example gives you $100.00!", "");
            // set the user's balance back to the original value.
            _serverProxy.SetUserAccountBalance(user, currentBalance, "Just teasing!", "");

            // disable the user's printing for 5 minutes
            _serverProxy.DisablePrintingForUser(user, 5);

            // set some properties of the user
            _serverProxy.SetUserProperty(user, "card-number", "TEST-1234");
            _serverProxy.SetUserProperty(user, "department", "Department of Testing");
            _serverProxy.SetUserProperty(user, "email", "test@user.id");
            _serverProxy.SetUserProperty(user, "full-name", "Sir Testalot");
            _serverProxy.SetUserProperty(user, "username-alias", user + "-alias");
            _serverProxy.SetUserProperty(user, "notes", "This is just a test user");
            _serverProxy.SetUserProperty(user, "office", "Office of High Velocity Impact");
            _serverProxy.SetUserProperty(user, "restricted", "FALSE");


            // Set the user's account overdraft mode

            _serverProxy.SetUserOverdraftMode(user, "individual");


            // print out the user's details
            Console.WriteLine();
            Console.WriteLine("Printing out user details:");
            Console.WriteLine("  User: " + user + " (" + _serverProxy.GetUserProperty(user, "full-name") + ")");
            Console.WriteLine("  Balance: " + _serverProxy.GetUserProperty(user, "balance"));
            Console.WriteLine("  Email: " + _serverProxy.GetUserProperty(user, "email"));
            Console.WriteLine("  Department: " + _serverProxy.GetUserProperty(user, "department"));
            Console.WriteLine("  Office: " + _serverProxy.GetUserProperty(user, "office"));
            Console.WriteLine("  Card Number: " + _serverProxy.GetUserProperty(user, "card-number"));
            Console.WriteLine("  Print Jobs: " + _serverProxy.GetUserProperty(user, "print-stats.job-count"));
            Console.WriteLine("  Pages Printed: " + _serverProxy.GetUserProperty(user, "print-stats.page-count"));
            Console.WriteLine("  Printing Disabled: " + _serverProxy.GetUserProperty(user, "disabled-print"));
            Console.WriteLine("  Restricted: " + _serverProxy.GetUserProperty(user, "restricted"));
            Console.WriteLine("  Notes: " + _serverProxy.GetUserProperty(user, "notes"));
            Console.WriteLine("  Overdraft Mode: " + _serverProxy.GetUserOverdraftMode(user));
            Console.WriteLine();

            Console.WriteLine("Now look up the user name by card number, email, alias and full name");
            Console.WriteLine("card number: " + _serverProxy.LookUpUserNameByCardNo("TEST-1234"));
            Console.WriteLine("email: " + _serverProxy.LookUpUserNameByEmail("test@user.id"));
            Console.WriteLine("alias: " + _serverProxy.LookUpUserNameBySecondaryUserName(user + "-alias"));
            Console.WriteLine("full name: " + _serverProxy.LookUpUsersByFullName("Sir Testalot")[0]);
 

            // Perform some transactions by card-number
            _serverProxy.AdjustUserAccountBalanceByCardNumber("TEST-1234", 100.0, "Congratulations!");
            _serverProxy.AdjustUserAccountBalanceByCardNumber("TEST-1234", -100.0, "Easy come, easy go.");

            // enable the user's account again
            Console.WriteLine("Enabling the user's printing again...");
            _serverProxy.SetUserProperty(user, "disabled-print", "FALSE");
            Console.WriteLine("  Printing Disabled: " + _serverProxy.GetUserProperty(user, "disabled-print"));
            Console.WriteLine();



            Console.WriteLine("About to export the test user data ");
            _serverProxy.ExportUserDataHistory(user, "c:\\Program Files\\PaperCut MF\\server\\data\\content\\" );



            Console.WriteLine("User Example Complete.");
            Console.WriteLine("View " + user + "'s settings and transaction log.");
            Console.WriteLine();
            Console.WriteLine();
        } catch (XmlRpcFaultException fex) {
            Console.WriteLine("Fault: {0}, {1}", fex.FaultCode, fex.FaultString);
        }
    }

    private static void SharedAccountExample(string user, string account) {

        //
        // SHARED ACCOUNT EXAMPLE
        //
        Console.WriteLine();
        Console.WriteLine("--------------------------");
        Console.WriteLine("- Shared Account Example -");
        Console.WriteLine("--------------------------");
        Console.WriteLine();
        try {
            if (_serverProxy.SharedAccountExists(account)) {
                Console.WriteLine("The test shared account already exists.");
            } else {
                // shared account does not exist so create
                Console.WriteLine("Creating shared account: " + account);
                _serverProxy.AddNewSharedAccount(account);
            }

            // start the account with $100.00
            _serverProxy.SetSharedAccountAccountBalance(account, 100.00, "Starting balance");

            // subtract some credit
            _serverProxy.AdjustSharedAccountAccountBalance(account, -10.00, "Lunch money");

            // allow our test user to charge to this account without a PIN
            _serverProxy.AddSharedAccountAccessUser(account, user);

            // disable the shared account for 5 minutes
            _serverProxy.DisableSharedAccount(account, 5);

            // set up the account properties
            _serverProxy.SetSharedAccountProperty(account, "comment-option", "COMMENT_REQUIRED");
            _serverProxy.SetSharedAccountProperty(account, "invoice-option", "NEVER_INVOICE");
            _serverProxy.SetSharedAccountProperty(account, "notes", "This is a test account.");
            _serverProxy.SetSharedAccountProperty(account, "pin", "9H37X");
            _serverProxy.SetSharedAccountProperty(account, "restricted", "TRUE");

            // Set the account overdraft mode

            _serverProxy.SetSharedAccountOverdraftMode(account, "individual");

            // print out the account's details
            Console.WriteLine();
            Console.WriteLine("Printing out shared account details:");
            Console.WriteLine("  Account: " + account);
            Console.WriteLine("  Access Groups: " + _serverProxy.GetSharedAccountProperty(account, "access-groups"));
            Console.WriteLine("  Access Users: " + _serverProxy.GetSharedAccountProperty(account, "access-users"));
            Console.WriteLine("  Account Id: " + _serverProxy.GetSharedAccountProperty(account, "account-id"));
            Console.WriteLine("  Balance: " + _serverProxy.GetSharedAccountProperty(account, "balance"));
            Console.WriteLine("  Commenting: " + _serverProxy.GetSharedAccountProperty(account, "comment-option"));
            Console.WriteLine("  Disabled: " + _serverProxy.GetSharedAccountProperty(account, "disabled"));
            Console.WriteLine("  Invoicing: " + _serverProxy.GetSharedAccountProperty(account, "invoice-option"));
            Console.WriteLine("  Notes: " + _serverProxy.GetSharedAccountProperty(account, "notes"));
            Console.WriteLine("  Overdraft Mode: " + _serverProxy.GetSharedAccountOverdraftMode(account));
            Console.WriteLine("  Overdraft Amount: " + _serverProxy.GetSharedAccountProperty(account, "overdraft-amount"));
            Console.WriteLine("  PIN: " + _serverProxy.GetSharedAccountProperty(account, "pin"));
            Console.WriteLine("  Restricted: " + _serverProxy.GetSharedAccountProperty(account, "restricted"));
            
            Console.WriteLine();
            Console.WriteLine("Shared Account Example Complete.");
            Console.WriteLine("View " + account + "'s settings and transaction log.");
            Console.WriteLine();

            Console.WriteLine("About to delete the test user and redact data");
            _serverProxy.DeleteExistingUser(user, true);
        } catch (XmlRpcFaultException fex) {
            Console.WriteLine("Fault: {0}, {1}", fex.FaultCode, fex.FaultString);
        }
    }
}
