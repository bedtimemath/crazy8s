WITH
[SurveyResultAnswers] AS (
	SELECT sr.[Id] AS [SurveyResultId], sr.[SessionId], CAST(sr.[Completed] AS DATE) AS [Completed], 
		a.[QuestionId], COALESCE(sel.[Text], opt.[Text], a.[Text]) AS [Answer]
	FROM [Bits].[SurveyResult] sr
	JOIN [Bits].[Answer] a ON a.[SurveyResultId] = sr.[Id]
	LEFT JOIN [Bits].[AnswerSelection] sel ON sel.[AnswerId] = a.[Id]
	LEFT JOIN [Bits].[AnswerOption] opt ON opt.[Id] = sel.[OptionId]
)
SELECT DISTINCT sra.[SurveyResultId], sra.[SessionId], sra.[Completed], 
	q.[Text] AS [Question], sra.[Answer], ss.[Ordinal], q.[Ordinal]
FROM [SurveyResultAnswers] sra
JOIN [Bits].[Question] q ON q.[Id] = sra.[QuestionId]
JOIN [Bits].[SurveySection] ss ON ss.[Id] = q.[SectionId]
WHERE sra.[SurveyResultId] = '3009BB40-BC1F-472C-8DCA-082B67FA4A26'
ORDER BY ss.[Ordinal], q.[Ordinal];

