SELECT w.Id, w.Date, w.Duration, w.DogId, w.WalkerId, d.Name AS DogName, o.Name AS OwnerName, o.Id AS OwnerId
                        FROM Walks as w
                        LEFT JOIN Dog as d ON d.Id = w.DogId
                        LEFT JOIN Owner as o ON o.Id = d.OwnerId
                        WHERE w.WalkerId = 2