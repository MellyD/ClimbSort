insert into WallType (Description,CreatedDate,ModifiedDate) values ('Steep',GETDATE(),GETDATE()), ('Roof',GETDATE(),GETDATE()), ('Vertical',GETDATE(),GETDATE()), ('Slab',GETDATE(),GETDATE()), ('Traverse',GETDATE(),GETDATE()), ('Other',GETDATE(),GETDATE())

insert into GradingSystem (Name,Discipline,CreatedDate,ModifiedDate) values ('Font', 1,GETDATE(),GETDATE()), ('V-Scale',1,GETDATE(),GETDATE()), ('British Tech',1,GETDATE(),GETDATE()), ('French',2,GETDATE(),GETDATE()), ('Peak',1,GETDATE(),GETDATE()), ('E-Scale',3,GETDATE(),GETDATE()),('British Tech',2,GETDATE(),GETDATE()),('USA Sport',2,GETDATE(),GETDATE()),('Norwegian',2,GETDATE(),GETDATE()),('Australian',2,GETDATE(),GETDATE()),('South African',2,GETDATE(),GETDATE())

DECLARE @Now DATETIME2 = GETDATE();

INSERT INTO Grade
(
    CreatedDate,
    ModifiedDate,
    GradeLabel,
    GradingSystemId,
    ScaleOrder,
    MinDifficultyRank,
    MaxDifficultyRank,
    MeanDifficultyRank
)
VALUES

------------------------------------------------------------
-- FONTAINEBLEAU (GradingSystemId = 1)
------------------------------------------------------------

(@Now,@Now,'1A',1, 1,  0.5,  1.5,  1.0),
(@Now,@Now,'1B',1, 2,  1.0,  2.0,  1.5),
(@Now,@Now,'1C',1, 3,  1.5,  2.5,  2.0),

(@Now,@Now,'2A',1, 4,  2.0,  3.0,  2.5),
(@Now,@Now,'2B',1, 5,  2.5,  3.5,  3.0),
(@Now,@Now,'2C',1, 6,  3.0,  4.0,  3.5),

(@Now,@Now,'3A',1, 7,  3.5,  4.5,  4.0),
(@Now,@Now,'3B',1, 8,  4.0,  5.0,  4.5),
(@Now,@Now,'3C',1, 9,  4.5,  5.5,  5.0),

(@Now,@Now,'4A',1,10,  5.0,  6.0,  5.5),
(@Now,@Now,'4B',1,11,  5.5,  6.5,  6.0),
(@Now,@Now,'4C',1,12,  6.0,  7.0,  6.5),

(@Now,@Now,'5A',1,13,  6.5,  7.5,  7.0),
(@Now,@Now,'5B',1,14,  7.5,  8.5,  8.0),
(@Now,@Now,'5C',1,15,  8.5,  9.5,  9.0),

(@Now,@Now,'6A',1,16,  9.5, 10.5, 10.0),
(@Now,@Now,'6A+',1,17,10.5, 11.5, 11.0),
(@Now,@Now,'6B',1,18,11.5, 12.5, 12.0),
(@Now,@Now,'6B+',1,19,12.5, 13.5, 13.0),
(@Now,@Now,'6C',1,20,13.5, 14.5, 14.0),
(@Now,@Now,'6C+',1,21,14.5, 15.5, 15.0),

(@Now,@Now,'7A',1,22,15.0, 16.0, 15.5),
(@Now,@Now,'7A+',1,23,16.0, 17.0, 16.5),
(@Now,@Now,'7B',1,24,17.0, 18.0, 17.5),
(@Now,@Now,'7B+',1,25,18.0, 19.0, 18.5),
(@Now,@Now,'7C',1,26,19.0, 20.0, 19.5),
(@Now,@Now,'7C+',1,27,20.0, 21.0, 20.5),

(@Now,@Now,'8A',1,28,21.0, 22.0, 21.5),
(@Now,@Now,'8A+',1,29,22.0, 23.0, 22.5),
(@Now,@Now,'8B',1,30,23.0, 24.0, 23.5),
(@Now,@Now,'8B+',1,31,24.0, 25.0, 24.5),
(@Now,@Now,'8C',1,32,25.0, 26.0, 25.5),
(@Now,@Now,'8C+',1,33,26.0, 27.0, 26.5),

(@Now,@Now,'9A',1,34,27.0, 28.0, 27.5);

INSERT INTO Grade
(
    CreatedDate,
    ModifiedDate,
    GradeLabel,
    GradingSystemId,
    ScaleOrder,
    MinDifficultyRank,
    MaxDifficultyRank,
    MeanDifficultyRank
)
VALUES

------------------------------------------------------------
-- V SCALE (GradingSystemId = 2)
------------------------------------------------------------

(@Now,@Now,'VB',  2, 1,  5.0,  6.5,  5.75),  -- ~ Font 4A-4C
(@Now,@Now,'V0-', 2, 2,  6.0,  7.5,  6.75),  -- ~ Font 4C-5A
(@Now,@Now,'V0',  2, 3,  7.0,  8.5,  7.75),  -- ~ Font 5A-5B
(@Now,@Now,'V0+', 2, 4,  8.0,  9.5,  8.75),  -- ~ Font 5B-5C
(@Now,@Now,'V1',  2, 5,  8.5, 10.0,  9.25),  -- ~ Font 5C-6A

(@Now,@Now,'V2',  2, 6,  9.5, 10.5, 10.0),   -- ~ Font 6A
(@Now,@Now,'V3',  2, 7, 10.5, 11.5, 11.0),   -- ~ Font 6A+
(@Now,@Now,'V4',  2, 8, 11.5, 12.5, 12.0),   -- ~ Font 6B
(@Now,@Now,'V5',  2, 9, 12.5, 13.5, 13.0),   -- ~ Font 6B+
(@Now,@Now,'V6',  2,10, 13.5, 15.0, 14.25),  -- ~ Font 6C

(@Now,@Now,'V7',  2,11, 14.5, 16.0, 15.25),  -- ~ Font 6C+/7A
(@Now,@Now,'V8',  2,12, 16.0, 17.5, 16.75),  -- ~ Font 7A+/7B
(@Now,@Now,'V9',  2,13, 17.5, 19.0, 18.25),  -- ~ Font 7B+
(@Now,@Now,'V10', 2,14, 19.0, 20.0, 19.5),   -- ~ Font 7C
(@Now,@Now,'V11', 2,15, 20.0, 21.0, 20.5),   -- ~ Font 7C+

(@Now,@Now,'V12', 2,16, 21.0, 22.0, 21.5),   -- ~ Font 8A
(@Now,@Now,'V13', 2,17, 22.0, 23.0, 22.5),   -- ~ Font 8A+
(@Now,@Now,'V14', 2,18, 23.0, 24.0, 23.5),   -- ~ Font 8B
(@Now,@Now,'V15', 2,19, 24.0, 25.0, 24.5),   -- ~ Font 8B+
(@Now,@Now,'V16', 2,20, 25.0, 26.5, 25.75),  -- ~ Font 8C / 8C+

(@Now,@Now,'V17', 2,21, 27.0, 28.0, 27.5);   -- ~ Font 9A


INSERT INTO Grade
(
    CreatedDate,
    ModifiedDate,
    GradeLabel,
    GradingSystemId,
    ScaleOrder,
    MinDifficultyRank,
    MaxDifficultyRank,
    MeanDifficultyRank
)
VALUES

(@Now,@Now,'E1',  6, 1,  6.0,  7.0,  6.5),
(@Now,@Now,'E2',  6, 2,  7.0,  8.0,  7.5),
(@Now,@Now,'E3',  6, 3,  8.0,  9.0,  8.5),
(@Now,@Now,'E4',  6, 4,  9.0, 10.5,  9.75),
(@Now,@Now,'E5',  6, 5, 10.5, 12.0, 11.25),

(@Now,@Now,'E6',  6, 6, 12.0, 14.0, 13.0),
(@Now,@Now,'E7',  6, 7, 14.0, 16.0, 15.0),
(@Now,@Now,'E8',  6, 8, 16.0, 18.0, 17.0),
(@Now,@Now,'E9',  6, 9, 18.0, 20.0, 19.0),

(@Now,@Now,'E10', 6,10, 20.0, 22.0, 21.0),
(@Now,@Now,'E11', 6,11, 22.0, 24.0, 23.0),
(@Now,@Now,'E12', 6,12, 24.0, 26.0, 25.0);

INSERT INTO Grade
(
    CreatedDate,
    ModifiedDate,
    GradeLabel,
    GradingSystemId,
    ScaleOrder,
    MinDifficultyRank,
    MaxDifficultyRank,
    MeanDifficultyRank
)
VALUES

(@Now,@Now,'1',   4, 1,  0.5,  1.5,  1.0),
(@Now,@Now,'2',   4, 2,  1.0,  2.0,  1.5),
(@Now,@Now,'3a',  4, 3,  1.5,  2.5,  2.0),
(@Now,@Now,'3b',  4, 4,  2.0,  3.0,  2.5),
(@Now,@Now,'3c',  4, 5,  2.5,  3.5,  3.0),

(@Now,@Now,'4a',  4, 6,  3.0,  4.0,  3.5),
(@Now,@Now,'4b',  4, 7,  3.5,  4.5,  4.0),
(@Now,@Now,'4c',  4, 8,  4.0,  5.0,  4.5),

(@Now,@Now,'5a',  4, 9,  5.0,  6.0,  5.5),
(@Now,@Now,'5b',  4,10,  6.0,  7.0,  6.5),
(@Now,@Now,'5c',  4,11,  7.0,  8.0,  7.5),

(@Now,@Now,'6a',  4,12,  8.0,  9.0,  8.5),
(@Now,@Now,'6a+', 4,13,  9.0, 10.0,  9.5),
(@Now,@Now,'6b',  4,14, 10.0, 11.0, 10.5),
(@Now,@Now,'6b+', 4,15, 11.0, 12.0, 11.5),
(@Now,@Now,'6c',  4,16, 12.0, 13.0, 12.5),
(@Now,@Now,'6c+', 4,17, 13.0, 14.0, 13.5),

(@Now,@Now,'7a',  4,18, 14.0, 15.0, 14.5),
(@Now,@Now,'7a+', 4,19, 15.0, 16.0, 15.5),
(@Now,@Now,'7b',  4,20, 16.0, 17.0, 16.5),
(@Now,@Now,'7b+', 4,21, 17.0, 18.0, 17.5),
(@Now,@Now,'7c',  4,22, 18.0, 19.0, 18.5),
(@Now,@Now,'7c+', 4,23, 19.0, 20.0, 19.5),
(@Now,@Now,'8a',  4,24, 20.0, 21.0, 20.5),
(@Now,@Now,'8a+', 4,25, 21.0, 22.0, 21.5),
(@Now,@Now,'8b',  4,26, 22.0, 23.0, 22.5),
(@Now,@Now,'8b+', 4,27, 23.0, 24.0, 23.5),
(@Now,@Now,'8c',  4,28, 24.0, 25.0, 24.5),
(@Now,@Now,'8c+', 4,29, 25.0, 26.0, 25.5),

(@Now,@Now,'9a',  4,30, 26.0, 27.0, 26.5),
(@Now,@Now,'9a+', 4,31, 27.0, 28.0, 27.5),
(@Now,@Now,'9b',  4,32, 28.0, 29.0, 28.5),
(@Now,@Now,'9b+', 4,33, 29.0, 30.0, 29.5),
(@Now,@Now,'9c',  4,34, 30.0, 31.0, 30.5);

INSERT INTO Grade
(
    CreatedDate,
    ModifiedDate,
    GradeLabel,
    GradingSystemId,
    ScaleOrder,
    MinDifficultyRank,
    MaxDifficultyRank,
    MeanDifficultyRank
)
VALUES

(@Now,@Now,'5.6',  8, 1,  2.0,  3.0,  2.5),
(@Now,@Now,'5.7',  8, 2,  3.0,  4.0,  3.5),
(@Now,@Now,'5.8',  8, 3,  4.0,  5.0,  4.5),
(@Now,@Now,'5.9',  8, 4,  5.0,  6.0,  5.5),

(@Now,@Now,'5.10a',8, 5,  6.0,  7.0,  6.5),
(@Now,@Now,'5.10b',8, 6,  7.0,  8.0,  7.5),
(@Now,@Now,'5.10c',8, 7,  8.0,  9.0,  8.5),
(@Now,@Now,'5.10d',8, 8,  9.0, 10.0,  9.5),

(@Now,@Now,'5.11a',8, 9, 10.0, 11.0, 10.5),
(@Now,@Now,'5.11b',8,10, 11.0, 12.0, 11.5),
(@Now,@Now,'5.11c',8,11, 12.0, 13.0, 12.5),
(@Now,@Now,'5.11d',8,12, 13.0, 14.0, 13.5),

(@Now,@Now,'5.12a',8,13, 14.0, 15.0, 14.5),
(@Now,@Now,'5.12b',8,14, 15.0, 16.0, 15.5),
(@Now,@Now,'5.12c',8,15, 16.0, 17.0, 16.5),
(@Now,@Now,'5.12d',8,16, 17.0, 18.0, 17.5),

(@Now,@Now,'5.13a',8,17, 18.0, 19.0, 18.5),
(@Now,@Now,'5.13b',8,18, 19.0, 20.0, 19.5),
(@Now,@Now,'5.13c',8,19, 20.0, 21.0, 20.5),
(@Now,@Now,'5.13d',8,20, 21.0, 22.0, 21.5),

(@Now,@Now,'5.14a',8,21, 22.0, 23.0, 22.5),
(@Now,@Now,'5.14b',8,22, 23.0, 24.0, 23.5),
(@Now,@Now,'5.14c',8,23, 24.0, 25.0, 24.5),
(@Now,@Now,'5.14d',8,24, 25.0, 26.0, 25.5),

(@Now,@Now,'5.15a',8,25, 26.0, 27.0, 26.5),
(@Now,@Now,'5.15b',8,26, 27.0, 28.0, 27.5),
(@Now,@Now,'5.15c',8,27, 28.0, 29.0, 28.5),
(@Now,@Now,'5.15d',8,28, 29.0, 30.0, 29.5);