# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.1] - 2023-04-14

### Added

- `CooldownRepository` class class for storing cooldowns of clicks.
- `CooldownCommand` class that uses `CooldownRepository` to implement a сooldown check before next commands.

## [1.1.0] - 2023-04-11

### Added

- `TaskReward` class.
- Required `TaskReward` argument to the constructor of the `TaskList` class.
- Required TaskReward argument to the `JsonTaskListSave.Load()` method.
- `IRewardCurrency` interface for accrual of reward currency.
- `IRewardValue` interface for calculating the amount of reward for completing a task.

## [1.0.0] - 2023-04-04

Initial release.