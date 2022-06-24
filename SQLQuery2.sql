SELECT w.Date, w.Duration, w.DogId, d.Name AS DogName, o.Name AS OwnerName, o.Id AS OwnerId
FROM Walks as w
LEFT JOIN Dog as d ON d.Id = w.DogId
LEFT JOIN Owner as o ON o.Id = w.DogId
WHERE w.WalkerId = 1