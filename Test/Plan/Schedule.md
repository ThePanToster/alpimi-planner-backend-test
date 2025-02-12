# Schedule entity test plan

## `ALL` `api/Schedule/*`

- [ScheduleControllerThrowsUnauthorized()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when token is not provided

- [ScheduleControllerThrowsTooManyRequests()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when request is sent too many times


## `POST` `api/Schedule`

- [ScheduleIsCreated()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if schedule is created when provided with correct data

- [ThrowsErrorWheNameIsTaken()](../Entities/ESchedule/Commands/CreateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when a taken name is provided

- [ThrowsErrorWhenDateStartIsAfterDateEnd()](../Entities/ESchedule/Commands/CreateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when start date is after end date

- [ThrowsErrorWhenSchoolHourIsLessThan1()](../Entities/ESchedule/Commands/CreateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when school hour is less than 1

- [ThrowsErrorWhenSchoolHourIsLessThan1440()](../Entities/ESchedule/Commands/CreateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when school hour is more than 1440

- [ThrowsErrorWhenSchoolDaysLengthIsOtherThan7()](../Entities/ESchedule/Commands/CreateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when school days length is other than seven

- [ThrowsErrorWhenSchoolDaysContainsSomethingOtherThan1Or0()](../Entities/ESchedule/Commands/CreateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when school days contains symbols other than 1 or 0

- [ThrowsErrorWhenNameContainsIllegalAnSymbol](../Entities/ESchedule/Commands/CreateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when name contains an illegal symbol


## `DELETE` `api/Schedule/{id}`

- [ScheduleIsDeleted()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if schedule is deleted when a valid token is provided


## `PATCH` `api/Schedule/{id}`

- [UpdateScheduleReturnsUpdatedSchedule()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an updated schedule when provided with correct data

- [UpdateScheduleThrowsNotFoundErrorWhenWrongIdIsGiven()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when schedule doesn't exists

- [UpdateScheduleThrowsNotFoundErrorWhenWrongUserAttemptsUpdate()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when schedule is inaccessible for user

- [ThrowsErrorWhenURLAlreadyExists()](../Entities/ESchedule/Commands/UpdateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when url is already taken

- [ThrowsErrorWhenNameContainsIllegalAnSymbol](../Entities/ESchedule/Commands/UpdateScheduleCommand.unit.cs) - **unit**  
  Check if returns an error when name contains an illegal symbol

## `GET` `api/Schedule`

- [GetAllScheduleReturnsSchedules()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns two schedules when a valid token is provided

- [GetAllScheduleReturnsEmptyContentWhenWrongUserAttemptsGet()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns no schedules when other user's token is provided

- [GetAllScheduleReturnsOnlyUserMadeSchedules()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns a schedule when valid token is provided

- [ThrowsErrorWhenIncorrectPerPageIsGiven()](../Entities/ESchedule/Queries/GetSchedulesQuery.unit.cs) - **unit**  
  Check if returns an error when provided with invalid perPage

- [ThrowsErrorWhenIncorrectPageIsGiven()](../Entities/ESchedule/Queries/GetSchedulesQuery.unit.cs) - **unit**  
  Check if returns an error when provided with invalid page

- [ThrowsErrorWhenIncorrectSortByIsGiven()](../Entities/ESchedule/Queries/GetSchedulesQuery.unit.cs) - **unit**  
  Check if returns an error when provided with invalid sortBy

- [ThrowsErrorWhenIncorrectSortOrderIsGiven()](../Entities/ESchedule/Queries/GetSchedulesQuery.unit.cs) - **unit**  
  Check if returns an error when provided with invalid sortOrder

- [ThrowsMultipleErrorMessages()](../Entities/ESchedule/Queries/GetSchedulesQuery.unit.cs) - **unit**  
  Check if returns multiple errors when provided with multiple invalid parameters


  ## `GET` `api/Schedule/byName/{name}`

- [GetScheduleByNameReturnsSchedule()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns a schedule when a valid token is provided

- [GetScheduleByNameThrowsNotFoundErrorWhenWrongUserTokenIsGiven())](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when a schedule is inaccessible for user

- [GetScheduleByNameThrowsNotFoundWhenWrongIdIsGiven()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when a schedule doesn't exists


## `GET` `api/Schedule/{id}`

- [GetScheduleReturnsSchedule()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns a schedule when a valid token is provided

- [GetScheduleThrowsNotFoundErrorWhenWrongUserTokenIsGiven()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when a schedule is inaccessible for user

- [GetScheduleThrowsNotFoundWhenWrongIdIsGiven()](../Entities/ESchedule/ScheduleController.test.cs) - **integrity**  
  Check if returns an error when a schedule doesn't exists

