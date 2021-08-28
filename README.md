# SocialPlatform
Demo project for chat group feature build on top of Service Fabric. 

## Description
This demo project implements simple social group service that allows users to list, create, join and leave groups. Once inside a group, user can send messages to the group which stores them in ReliableDictionary and sends the messages to all other group members who are online. In production a proper queryable database should be used for storing the groups and messages to allow queries. Currently all the messages are stored in the actors memory and returned when player requests messages. This will cause issues once the messages amount grows and instead of returning all messages only a subset of messages should be returned and support for pagination and filtering should be added.

The main components of the solution are
**Group API** handles the WebSocket connections and message routing to other services.
**Group Registry** stores and serves clients endpoint for fetching list all groups.
**Group Actor** hosts the actual actor based group implementation and also offers actor event based notification system for API to receive relevant messages.

Also the solution includes a very simple console client for testing the solution.

![Image depicting the console client](https://user-images.githubusercontent.com/15885524/131219795-4c59e42e-0198-4a85-9e54-0225e78393fb.PNG)

### Communication
Communication between client and and API is done with TCP over WebSockets. All messages sent over WebSockets are binary serialized using MessagePack serialization library and prepended with a byte indicating the message type (defined in ClientToServerMessageType.cs and ServerToClientMessageType.cs). API communicates with Group Registry using service proxy. API and Group Actor communicate together via actor proxy and actor events to generate two-way communication channel. 

![Image depicting the communication between different parts of the solution](https://user-images.githubusercontent.com/15885524/131219791-4bf017bd-07d2-4fa7-a6dd-b768888987ba.PNG)

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
