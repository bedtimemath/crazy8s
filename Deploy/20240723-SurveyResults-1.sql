SELECT sr.[Id] AS [SurveyResultId], u.[Name], u.[Email], CAST(sr.[Completed] AS DATE) AS [Completed]
FROM [Bits].[SurveyResult-Active] sr
JOIN [Bits].[AppSession] ass ON ass.[Id] = sr.[SessionId]
JOIN [Bits].[User] u ON u.[Id] = ass.[UserId]
WHERE sr.[Completed] IS NOT NULL AND sr.[Completed] > '2022-08-02'
;

WITH
[Answers] AS (
	SELECT sr.[Id] AS [SurveyResultId], 
		a.[QuestionId], COALESCE(sel.[Text], opt.[Text], a.[Text], '') AS [Answer]
	FROM [Bits].[SurveyResult] sr
	JOIN [Bits].[Answer] a ON a.[SurveyResultId] = sr.[Id]
	LEFT JOIN [Bits].[AnswerSelection] sel ON sel.[AnswerId] = a.[Id]
	LEFT JOIN [Bits].[AnswerOption] opt ON opt.[Id] = sel.[OptionId]
)
SELECT [SurveyResultId], [QuestionId], STRING_AGG([Answer],',') AS [Answer]
FROM [Answers]
GROUP BY [SurveyResultId], [QuestionId]
;