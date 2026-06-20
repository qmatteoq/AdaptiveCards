# Adaptive Cards samples

Different samples that show how to render and use **Adaptive Cards** across Windows and
the Microsoft ecosystem, including the Windows 10 **Timeline** feature.

> **Historical sample.** Targets UWP / Desktop Bridge on Windows 10 and the bot/Azure
> Functions stacks of the time. The Adaptive Cards concepts still apply; SDKs and hosting
> have since evolved.

## What's included

| Project | What it demonstrates |
| --- | --- |
| `AdaptiveCards.Package` | A WPF app packaged with the Desktop Bridge that renders Adaptive Cards and pushes activities to the Windows Timeline. |
| `AdaptiveCards.Bot` | A bot that returns Adaptive Cards in conversations. |
| `AdaptiveCards.Function` | An Azure Function that produces / serves Adaptive Card payloads. |

## Prerequisites

- Windows 10
- Visual Studio 2017 with the **UWP**, **Azure** and **.NET desktop** workloads
- An Azure subscription (for the bot and Function projects)

## Getting started

1. Clone the repository and open the solution.
2. Pick the project you want to explore (packaged app, bot or function).
3. Restore packages, supply any required configuration and run.

## License

Released under the [MIT License](LICENSE).
