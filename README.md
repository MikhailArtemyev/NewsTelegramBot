# AllTagBot - Telegram News Bot

My attempt to build a Telegram bot using .NET. The bot delivers news updates and provides user registration functionality.
This is a prototype version meant to demonstrate the core architecture and functionality.

## Features

- **News Distribution** - Automated news posting and updates
- **User Registration** - Complete user registration and management system
- **Message Routing** - Intelligent message handling and command routing
- **Configuration Management** - JSON-based configuration system

## Prerequisites

- .NET 6.0 or higher
- Telegram Bot Token (obtained from [@BotFather](https://t.me/botfather))

## Installation

1. **Clone the repository**
   ```bash
   git clone <your-repository-url>
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the bot**
   - Create a `config.json` file in the project root
   - Add your bot configuration (see [Configuration](#configuration) section)

4. **Build the project**
   ```bash
   dotnet build
   ```

## Configuration

Create a `config.json` file in the project root with the following structure:

```json
{
  "BotClientToken": "YOUR_BOT_TOKEN_HERE",
  "PublishKeyword": "#publish",
  "ChatPassword": "A_B_C_D_PASSWORD",
  "SuperAccess": "<<SuperAccessToken>>"
}
```

## License

This project is to be used for educational purposes only.

