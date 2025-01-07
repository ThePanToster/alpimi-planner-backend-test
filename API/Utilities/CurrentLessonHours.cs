using AlpimiAPI.Database;

namespace AlpimiAPI.Utilities
{
    public static class CurrentLessonHours
    {
        public static async Task Update(
            IDbService _dbService,
            Guid lessonId,
            CancellationToken cancellationToken
        )
        {
            int? amountOfHoursToInsert = await _dbService.Get<int?>(
                $@"
                    SELECT SUM([LessonEnd]-[LessonStart]+1)
                    FROM [LessonBlock]
                    WHERE [LessonId] = '{lessonId}';",
                ""
            );

            if (amountOfHoursToInsert == null)
            {
                amountOfHoursToInsert = 0;
            }

            await _dbService.Update<Guid?>(
                $@"
                    UPDATE [Lesson] 
                    SET
                    [CurrentHours] = {amountOfHoursToInsert}
                    OUTPUT
                    INSERTED.[Id]
                    WHERE [Id] = '{lessonId}';",
                ""
            );
        }
    }
}
