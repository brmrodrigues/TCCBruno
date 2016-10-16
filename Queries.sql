declare @paluno_id int = 6

SELECT COUNT(C.checkin_id) as PresencaCount, T.treino_tipo_nome as NomeSubTreino, C.aluno_id as AlunoId
FROM Checkin as C
INNER JOIN Treino_Tipo as T on (C.treino_tipo_id = T.treino_tipo_id)
GROUP BY T.treino_tipo_nome, C.aluno_id


