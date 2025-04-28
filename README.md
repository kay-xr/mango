# Mango 🥭

Mango is a simple server monitor and controller for controlling game servers, primarily steamcmd and Minecraft servers.

### The Idea 💡

Mango was initially built as a way to initiate updates and backups over the web. It quickly snowballed into me needing things like authentication, which took me down the path of building a full GUI for my servers that other users can access and manage instances. I host a lot of servers for my friends and the idea was to give some level of control over processes to them without needing to provide full access to the server / network.

Mango operates by providing wrapper programs compatible with all operating systems written in .NET, and can be controlled via REST API / Websocket endpoints. These wrapper programs allow for control over processes and backups outside of the child process. 

All of these wrapper programs are then registered to a main host API / database, where they can be controlled using a web interface. Both the main API and web interface are hosted in a "master" container.

### But Why? 🙋

I am well aware of other projects such as Pterodactyl, in fact I use some of these projects myself. However, these projects are geared towards quick container management, and in the case of Pterodactyl, limited to Docker containers. I run a lot of servers where running in a Docker environment might not be possible or make the most sense, therefore I decided to build a much more flexible, lower-level system that can eventually be exapanded to control almost any server process remotely.

If you are just looking for simple container management, or a way to quickly spin up and down game servers, check out Pterodactyl! It's much more suited for that sort of thing.

### Features (Future, and Currernt) 🛠️

- Quick setup and local management via a simple TUI.
- Remote management of processes, backups, and updates via REST API endpoints.
- Realtime remote access of server consoles via Websockets.
- Simple web interface for quickly managing server instances on the go.
- Access controls for external user access to specific instances, allowing friends to manage remote servers.
- Secure configuration edits.

### Currently Supports 🪜

- SteamCMD servers (Updating, Backups, Process Management, Config Management)
- Minecraft servers (Backups, Process Management, Config Management)
- Almost any server providing SDOUT/IN (Backups, Process Management)

> [!CAUTION]  
> Mango is a work-in-progress and is built for internal use only! Security vulnerabilities may be present, especially due to the nature, and level of access this project gives to servers. It is in no way suitable for production use, and is soley built as a demonstration project. I take absolutely no responsibility in the security of your use with this codebase.