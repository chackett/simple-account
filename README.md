# simple-account
SimpleAccount is a simple bank account - It merges all your accounts into just one simple account. Built with C# .NET Core.  

This webservice has a concept of a user (userId), where accounts from various bank accounts are linked to. The API endpoints return a merged view of all the linked accounts, so transactions and accounts are all returned in single calls.

There are no hardcoded values in here and multiple users can be used simultaneoulsy along with multiple accounts being linked to each user.

This implementation stores everything in memory, so account consent will need to be performed for each run of the process.

# Usage

## Prerequisites

* .NET Core SDK (I used 3.1 on Windows 10)
* Docker (I used Docker Desktop 2.2.0.5, Windows 10)
* Rename `secrets.example.json` to `secrets.json` and ensure populated with values from TrueLayer console.
Ensure callback URL is specified in Truelyer console also.

## Build & Run
From `SimpleAccount` directory, run:  
`docker build -t "simpleaccount:latest" . && docker run -p 3000:3000 simpleaccount`

## Testing
Some unit tests have been written. Controllers are tested to cover typical use cases and the services tested to cover atypical use cases.  
  
To test the project run `dotnet test` from the repository root.

## Endpoints

The refresh URL value will force a call to the upstream API (TrueLayer) if true. If false, the local store will be read, as long as data has previously been put there.

Get authorisation link, to add a bank account.  
GET `http://127.0.0.1:3000/consent/authorise?userId=demoUser`

Get linked accounts, set refresh to will force a new call to TrueLayer API, instead of using local cache.  
GET `http://localhost:3000/account/accounts?userId=demoUser&refresh=false`

Get transactions for specified account. AccountId can be retrieved from accounts endpoint response.  
GET `http://localhost:3000/account/transactions?userId=demoUser&refresh=false`

Get a sevenday summary of expenditure. Summarrises expense by merchant name.  
GET `http://localhost:3000/analytics/sevendaysummary?userId=demoUser&refresh=false`

TrueLayer consent callback URL - TrueLayer will POST back to this URL during consent process  
POST `http://localhost:3000/consent/callback`