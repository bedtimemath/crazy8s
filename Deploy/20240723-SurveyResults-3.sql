WITH 
[SurveyQuestions] AS (
SELECT s.[Id] AS [SurveyId], s.[Name] AS [Survey], 
	ss.[Id] AS [SectionId], ss.[Name] AS [Section], ss.[Ordinal] AS [SectionOrdinal], 
	q.[Id] AS [QuestionId], q.[Text] AS [Question], q.[Ordinal] AS [QuestionOrdinal]
FROM [Bits].[Survey-Active] s
JOIN [Bits].[SurveySection-Active] ss ON ss.[SurveyId] = s.[Id]
JOIN [Bits].[Question-Active] q ON q.[SectionId] = ss.[Id]
)
SELECT --sq.*,
	sr.[Id] AS [SurveyResultId], sr.[Completed], sr.[Note],
	u.[Name], u.[Email]
FROM [Bits].[SurveyResult-Active] sr
--JOIN [SurveyQuestions] sq ON sq.[SurveyId] = sr.[SurveyId]
LEFT JOIN [Bits].[AppSession] ap ON ap.[Id] = sr.[SessionId]
LEFT JOIN [Bits].[User] u ON u.[Id] = ap.[UserId]
WHERE sr.[SurveyId] = 'C7C467E4-17F3-4255-83E3-B963A72752B6'
	AND u.[Name] = 'Emily Savageau'
--ORDER BY [SectionOrdinal], [QuestionOrdinal]
;


SELECT 
	sr.[Id] AS [SurveyResultId], sr.[Completed], sr.[Note],
	u.[Id] AS [UserId], u.[Name], u.[Email]
FROM [Bits].[SurveyResult] sr
LEFT JOIN [Bits].[AppSession] ap ON ap.[Id] = sr.[SessionId]
LEFT JOIN [Bits].[User] u ON u.[Id] = ap.[UserId]
WHERE u.[Name] = 'Emily Savageau'
;

WITH
[SurveyAnswers] AS (
SELECT u.[Name], u.[Email], CAST(sr.[Completed] AS DATE) AS [Completed],
	s.[Name] AS [Survey], ss.[Name] AS [Section],
	q.[Id] AS [QuestionId],
	q.[Text] As [Question], t.[Name] AS [AnswerType], 
	COALESCE(sel.[Text], opt.[Text], a.[Text]) AS [Answer]
FROM [Bits].[SurveyResult] sr
JOIN [Bits].[AppSession] ap ON ap.[Id] = sr.[SessionId]
JOIN [Bits].[User] u ON u.[Id] = ap.[UserId]
JOIN [Bits].[Survey] s ON s.[Id] = sr.[SurveyId]
JOIN [Bits].[SurveySection] ss ON ss.[SurveyId] = s.[Id]
JOIN [Bits].[Question] q ON q.[SectionId] = ss.[Id]
JOIN [Bits].[AnswerType] t ON t.[Id] = q.[AnswerTypeId]
JOIN [Bits].[Answer] a ON a.[SurveyResultId] = sr.[Id] AND a.[QuestionId] = q.[Id]
LEFT JOIN [Bits].[AnswerSelection] sel ON sel.[AnswerId] = a.[Id]
LEFT JOIN [Bits].[AnswerOption] opt ON opt.[Id] = sel.[OptionId]
WHERE sr.[Completed] IS NOT NULL AND s.[Id] = 'C7C467E4-17F3-4255-83E3-B963A72752B6'
)
SELECT [Name], [Email], [Completed], COUNT(*)
FROM [SurveyAnswers]
WHERE [Completed] > '2022-08-02'
GROUP BY [Name], [Email], [Completed]
;

WITH
[UserResults] AS (
SELECT u.[Name], u.[Email], CAST(sr.[Completed] AS DATE) AS [Completed], 
	sr.[Id] AS [SurveyResultId], ss.[Name] AS [SectionName], ss.[Ordinal] AS [SectionOrdinal],
	q.[Id] AS [QuestionId], q.[Ordinal] AS [QuestionOrdinal], q.[Text] AS [Question],
	a.[Id] AS [AnswerId], COALESCE(sel.[Text], opt.[Text], a.[Text]) AS [Answer]
FROM [Bits].[User] u
JOIN [Bits].[AppSession] ass ON ass.[UserId] = u.[Id]
JOIN [Bits].[SurveyResult-Active] sr ON sr.[SessionId] = ass.[Id]
JOIN [Bits].[Answer] a ON a.[SurveyResultId] = sr.[Id] 
LEFT JOIN [Bits].[AnswerSelection] sel ON sel.[AnswerId] = a.[Id]
LEFT JOIN [Bits].[AnswerOption] opt ON opt.[Id] = sel.[OptionId]
JOIN [Bits].[Question] q ON q.[Id] = a.[QuestionId]
JOIN [Bits].[SurveySection] ss ON ss.[Id] = q.[SectionId]
WHERE sr.[Completed] IS NOT NULL
)
SELECT ur.[SurveyResultId], ur.[Name], ur.[Email], ur.[Completed], 
  (SELECT STRING_AGG([Answer],',') FROM [UserResults] WHERE [SurveyResultId] = ur.[SurveyResultId] AND [QuestionId] = '8B926255-0FD8-411D-AFAE-3E09EDDAADB7')
	AS [Select the Season of your most recent or current club:], 
  (SELECT STRING_AGG([Answer],',') FROM [UserResults] WHERE [SurveyResultId] = ur.[SurveyResultId] AND [QuestionId] = 'B9A39687-48B7-4B03-924E-C7725291C581')
	AS [Which grades participated in that club?]
FROM [UserResults] ur
ORDER BY ur.[Name], ur.[Email], ur.[Completed]
;



SELECT ur.[Name], ur.[Email], ur.[Completed],
-- 8B926255-0FD8-411D-AFAE-3E09EDDAADB7	Select the Season of your most recent or current club:
	(SELECT COALESCE(sel.[Text], opt.[Text], a.[Text])
		FROM [Bits].[SurveyResult-Active] sra
		JOIN [Bits].[Answer] a ON a.[SurveyResultId] = sra.[Id] AND a.[QuestionId] = '8B926255-0FD8-411D-AFAE-3E09EDDAADB7'
		LEFT JOIN [Bits].[AnswerSelection] sel ON sel.[AnswerId] = a.[Id]
		LEFT JOIN [Bits].[AnswerOption] opt ON opt.[Id] = sel.[OptionId]) 
	AS [Select the Season of your most recent or current club:]
FROM [UserResults] ur
ORDER BY ur.[Name], ur.[Email], ur.[Completed]
;