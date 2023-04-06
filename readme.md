# WebUntis

Yet another C# Wrapper for WebUntis Backend.

### (Web)Untis you say?

Untis is the one-stop-solution for schools to manage schools, classes, teachers, assets (rooms) etc.
For us parents you gain access to your kids school day, which classes are canceled, upcoming events and tests and so on.

### Why?

I wanted to integrate the Untis-Timetable into our home-automation-system to get

- easy access to information as to when kids are getting home or when their days starts,
- what upcoming events we need to be on the lookout for,
- adjusting their alarm clocks to cover for canceled morning-classes.

Yes, sure, there were and are plenty of these clients, some were stale, some did not provide the stuff I wanted. So starting from scratch meant I could add functionality to my liking.
So can you if you wish to ;)

## How to use

### Use my version

If you do not want to extend the functionality, simply install the nuget-package provided by the nuget manager and you are all set.
Refer to the TestSuite for help or use intellisense.

### Clone and change

Clone the repository, setup `appsettings.json` and you are good to go. Make your changes to WebUntis and publish it under your name and account or use it as a local library.

#### For debugging: `appsettings.json`

It should look like this

```
{
    "Username": "<email_you_registered_with@untis.tld",
    "Password": "<you_should_know>",
    "School": "<ident_of_your_school>",
    "ServerIdent": "<ident_of_your_server>",
    "ClientIdentifier": "<a_name_for_your_client>",
    "PupilId": <id_of_your_kid>
}
```

`Username` and `Password` should be self-explanatory.

For all other values you should use the official untis-web-login and with a little patience, some queries and the trusty Debugger Console you'll get the rest of the values.
There might be an easier way, but I am not aware of one.

`ServerIdent`: this looks to me to be some kind of a load-balancer, where _n_ schools are on one server and there are _x_ servers in the untis-net.  
So you would need the server- and school-ident-pair to get to your data.

`ClientIdentifier`: afaik it is used to identify your client on the server-side. While I am writing this, any simple string was accepted. I called mine 'incredible', I saw others called theirs 'unknown', 'anonymous' or 'awesome'. You get the picture.

`PupilId`: the id of your kid. If you have multiple kids on the same school and server (as did I), you can use the same session to grab different timetables. Most functions take a puplilId as an argument. In the `appsettings.json` it's just for the TestSuite.
