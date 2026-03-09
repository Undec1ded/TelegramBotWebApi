# TelegramBotWebApi

ASP.NET Core Web API backend for a Telegram bot with Telegram Bot API integration, database support, background services, and admin/user interaction flows.

## Overview

This project is a backend service for a Telegram bot built with ASP.NET Core.  
It handles Telegram updates, business logic, database operations, admin tools, quiz flows, voting features, event subscriptions, and reminder processing.

The project also has a dedicated UI/UX design prepared in Figma for related interfaces and future frontend/admin panel expansion.

## Features

- Telegram Bot API integration
- ASP.NET Core Web API backend
- User registration and interaction flows
- Admin interaction flows
- Quiz system
- Voting system
- Event subscription and reminder logic
- Social links and project information blocks
- Background services for scheduled processing
- Relational database support
- Docker-based deployment support

## Tech Stack

- C#
- ASP.NET Core
- Web API
- SQL
- Docker
- Telegram Bot API

## Project Structure

- `Services/` — business logic
- `Repositories/` — data access layer
- `Models/` — entities, requests, responses, Telegram models
- `TgBot/` — Telegram bot update pipeline and handlers
- `Migration/` — database migration scripts
- `Utils/` — helper and utility classes

## Main Modules

### User Features
- start flow
- profile / fullname flow
- event subscription
- question submission
- voting participation
- quiz participation
- social links and project info

### Admin Features
- admin menu
- add administrators
- event management
- news creation
- questions moderation
- quiz management
- voting management

## Architecture Notes

The project follows a layered backend structure:

- **API / bot update handling**
- **service layer**
- **repository layer**
- **database models**
- **background processing**

This separation makes the codebase easier to maintain, extend, and test.

## Figma Design

UI/UX design for this project is available in Figma:

`https://www.figma.com/design/BsqlCH09mB3rfSQq82qhlJ/Untitled?node-id=0-1&p=f&t=Z3VnLs5lOaaEnaZA-0`
