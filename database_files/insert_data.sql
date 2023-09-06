INSERT INTO `student2021130434`.`users` (`id_user`, `username`, `new_password`, `hashed_password`, `occupation`) VALUES (1, 'gabe', 'admin', NULL, 'admin');
INSERT INTO `student2021130434`.`users` (`id_user`, `username`, `new_password`, `hashed_password`, `occupation`) VALUES (2, 'strang', 'teacher', NULL, 'teacher');
INSERT INTO `student2021130434`.`users` (`id_user`, `username`, `new_password`, `hashed_password`, `occupation`) VALUES (3, 'matt', 'student', NULL, 'student');
INSERT INTO `student2021130434`.`users` (`id_user`, `username`, `new_password`, `hashed_password`, `occupation`) VALUES (4, 'xxx', 'xxx', NULL, 'admin');
INSERT INTO `student2021130434`.`users` (`id_user`, `username`, `new_password`, `hashed_password`, `occupation`) VALUES (5, 'yyy', 'yyy', NULL, 'student');
INSERT INTO `student2021130434`.`users` (`id_user`, `username`, `new_password`, `hashed_password`, `occupation`) VALUES (6, 'zzz', 'zzz', NULL, 'teacher');

INSERT INTO `student2021130434`.`details` (`id_detail`, `category`, `summary`, `description`) VALUES (1, 'Wireless', 'The wireless internet is not working', 'None of the computers in the classroom are able to connect to the internet');
INSERT INTO `student2021130434`.`details` (`id_detail`, `category`, `summary`, `description`) VALUES (2, 'Software', 'The computer has shut down and doesn\'t want to turn on', 'The laptop said it had 5% and a few minutes later it just shut downs out of nowwhere');
INSERT INTO `student2021130434`.`details` (`id_detail`, `category`, `summary`, `description`) VALUES (3, 'PCSchool', 'I can\'t logon to my PCschool/spider account on ranginet', 'I put in the correct password but it says wrong username/password. I keep putting in my password without errors.');

INSERT INTO `student2021130434`.`tickets` (`id_ticket`, `id_user`, `id_detail`, `assigned`) VALUES (1, 2, 1, 'unassigned');
INSERT INTO `student2021130434`.`tickets` (`id_ticket`, `id_user`, `id_detail`, `assigned`) VALUES (2, 2, 2, 'gabe');
INSERT INTO `student2021130434`.`tickets` (`id_ticket`, `id_user`, `id_detail`, `assigned`) VALUES (3, 3, 3, 'gabe');