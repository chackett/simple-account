# simple-account
SimpleAccount is a simple bank account - It merges all your accounts into just one simple account. Built with C# .NET Core.

# Usage

## Preequisites

* .NET Core SDK (I used 3.1 on Windows 10)
* Docker (I used Docker Desktop 2.2.0.5, Windows 10)
* Rename `secrets.example.json` to `secrets.json` and ensure populated with values from TrueLayer console.
Ensure callback URL is specified in Truelyer console also.

## Build & Run
From `SimpleAccount` directory.
`docker build -t "simpleaccount:latest" . && docker run -p 3000:3000 simpleaccount`

## Endpoints

Get authorisation link, to add a bank account.
GET `http://127.0.0.1:3000/consent/authorise?userId=demoUser`

Get linked accounts, set refresh to will force a new call to TrueLayer API, instead of using local cache.
GET `http://localhost:3000/account/accounts?userId=demoUser&refresh=false`

Get transactions for specified account. AccountId can be retrieved from accounts endpoint response.
GET `http://localhost:3000/account/transactions?accountId=##ACCOUNT-ID##&refresh=false&userId=demoUser`

Get a sevenday summary of expenditure. Summarrises expense by merchant name.
GET `http://localhost:3000/analysis/sevendaysummary?userId=demoUser&refresh=false`

TrueLayer consent callback URL - TrueLayer will POST back to this URL during consent process
POST `http://localhost:3000/consent/callback`