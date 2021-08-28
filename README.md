# SocialPlatform
Demo project for chat group feature build on top of Service Fabric. 

## Description
This demo project implements simple social group service that allows users to list, create, join and leave groups. Once inside a group, user can send messages to the group which stores them in ReliableDictionary and sends the messages to all other group members who are online. In production a proper queryable database should be used for storing the groups and messages to allow queries. Currently all the messages are stored in the actors memory and returned when player requests messages. This will cause issues once the messages amount grows and instead of returning all messages only a subset of messages should be returned and support for pagination and filtering should be added.

### Future development
- Database backing for group registry service and group actors.
- Support NetworkMessages larger then 4096 by copying the buffer into stream when dealing with large messages.
- Use tokens for user authentication. Current implementation trusts the client and is very vulnerable for user impersonation attacks.
- Standardize logging between different services.
- Support for SSL.

### Setup

### Visual Studio
1. Install Microsoft Azure Service Fabric SDK
2. Open the solution and run it
3. Start SocialPlatform.Groups.ConsoleClient to connect and test the solution.
