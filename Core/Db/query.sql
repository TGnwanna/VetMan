
INSERT INTO AspNetRoles VALUES (NewId(),'SuperAdmin','SuperAdmin',NEWID());
INSERT INTO AspNetRoles VALUES (NewId(),'CompanyAdmin','CompanyAdmin',NEWID());
INSERT INTO AspNetRoles VALUES (NewId(),'CompanyManager','CompanyManager',NEWID());
INSERT INTO AspNetRoles VALUES (NewId(),'CompanyStaff','CompanyStaff',NEWID());

INSERT INTO CommonDropDowns VALUES (1 ,'Cash',1,0, '20120618 10:34:09 AM' ,NULL);
INSERT INTO PaymentMeans VALUES (id ,'Cash',1,0, '20120618 10:34:09 AM' ,NULL);
INSERT INTO CommonDropDowns VALUES (3 ,'Male',1,0, '20120618 10:34:09 AM' ,NULL);
INSERT INTO CommonDropDowns VALUES (3 ,'Female',1,0, '20120618 10:34:09 AM' ,NULL);


INSERT INTO Breeds (SpeciesId, Name, Active, Deleted, DateCreated, CompanyBranchId)
VALUES
  (1, 'Affenpinscher', 1, 0, GETDATE(), NULL),
  (1, 'Airedale Terrier', 1, 0, GETDATE(), NULL),
  (1, 'Australian Cattle Dog', 1, 0, GETDATE(), NULL),
  (1, 'Australian Shepherd', 1, 0, GETDATE(), NULL),
  (1, 'Australian Terrier', 1, 0, GETDATE(), NULL),
  (1, 'Basset Hound', 1, 0, GETDATE(), NULL),
  (1, 'Beagle', 1, 0, GETDATE(), NULL),
  (1, 'Bichon Frise', 1, 0, GETDATE(), NULL),
  (1, 'Boston Terrier', 1, 0, GETDATE(), NULL),
  (1, 'Boxer', 1, 0, GETDATE(), NULL),
  (1, 'Bulldog', 1, 0, GETDATE(), NULL),
  (1, 'Cocker Spaniel', 1, 0, GETDATE(), NULL),
  (1, 'Chinese Crested', 1, 0, GETDATE(), NULL),
  (1, 'English Bulldog', 1, 0, GETDATE(), NULL),
  (1, 'English Springer Spaniel', 1, 0, GETDATE(), NULL),
  (1, 'French Bulldog', 1, 0, GETDATE(), NULL),
  (1, 'German Shepherd', 1, 0, GETDATE(), NULL),
  (1, 'Golden Retriever', 1, 0, GETDATE(), NULL),
  (1, 'Greyhound', 1, 0, GETDATE(), NULL),
  (1, 'Great Dane', 1, 0, GETDATE(), NULL),
  (1, 'Great Pyrenees', 1, 0, GETDATE(), NULL),
  (1, 'Italian Greyhound', 1, 0, GETDATE(), NULL),
  (1, 'Irish Setter', 1, 0, GETDATE(), NULL),
  (1, 'Japanese Chin', 1, 0, GETDATE(), NULL),
  (1, 'Labrador Retriever', 1, 0, GETDATE(), NULL),
  (1, 'Lhasa Apso', 1, 0, GETDATE(), NULL),
  (1, 'Mastiff', 1, 0, GETDATE(), NULL),
  (1, 'Newfoundland', 1, 0, GETDATE(), NULL),
  (1, 'Papillon', 1, 0, GETDATE(), NULL),
  (1, 'Pekingese', 1, 0, GETDATE(), NULL),
  (1, 'Pomeranian', 1, 0, GETDATE(), NULL),
  (1, 'Poodle', 1, 0, GETDATE(), NULL),
  (1, 'Pug', 1, 0, GETDATE(), NULL),
  (1, 'Rottweiler', 1 , 0, GETDATE(), NULL)

  
INSERT INTO Breeds (SpeciesId, Name, Active, Deleted, DateCreated, CompanyBranchId)
VALUES
(2, 'Maine Coon', 1, 0, GETDATE(), NULL),
(2, 'Exotic Shorthair', 1, 0, GETDATE(), NULL),
(2, 'Ragdoll', 1, 0, GETDATE(), NULL),
(2, 'Siamese', 1, 0, GETDATE(), NULL),
(2, 'British Shorthair', 1, 0, GETDATE(), NULL),
(2, 'Siamese', 1, 0, GETDATE(), NULL),
(2, 'Sphynx', 1, 0, GETDATE(), NULL),
(2, 'Persian', 1, 0, GETDATE(), NULL)



INSERT INTO Breeds (SpeciesId, Name, Active, Deleted, DateCreated, CompanyBranchId)
VALUES
(3, 'Aberdeen Angus', 1, 0, GETDATE(), NULL),
(3, 'Angus', 1, 0, GETDATE(), NULL),
(3, 'Beefmaster', 1, 0, GETDATE(), NULL),
(3, 'Braunvieh', 1, 0, GETDATE(), NULL),
(3, 'Brahman', 1, 0, GETDATE(), NULL),
(3, 'Brown Swiss', 1, 0, GETDATE(), NULL),
(3, 'Charbray', 1, 0, GETDATE(), NULL),
(3, 'Charolais', 1, 0, GETDATE(), NULL),
(3, 'Chiangus', 1, 0, GETDATE(), NULL),
(3, 'Gelbvieh', 1, 0, GETDATE(), NULL),
(3, 'Hereford', 1, 0, GETDATE(), NULL),
(3, 'Holstein', 1, 0, GETDATE(), NULL),
(3, 'Jersey', 1, 0, GETDATE(), NULL),
(3, 'Lincoln Red', 1, 0, GETDATE(), NULL),
(3, 'Lim-Flex', 1, 0, GETDATE(), NULL),
(3, 'Limousin', 1, 0, GETDATE(), NULL),
(3, 'Marbled', 1, 0, GETDATE(), NULL),
(3, 'Murray Grey', 1, 0, GETDATE(), NULL),
(3, 'Nelore', 1, 0, GETDATE(), NULL),
(3, 'Pinzgauer', 1, 0, GETDATE(), NULL),
(3, 'Piedmontese', 1,0, GETDATE(), NULL)



INSERT INTO Breeds (SpeciesId, Name, Active, Deleted, DateCreated, CompanyBranchId)
VALUES
(4, 'Alpine', 1, 0, GETDATE(), NULL),
(4, 'Anglo-Nubian', 1, 0, GETDATE(), NULL),
(4, 'Angora', 1, 0, GETDATE(), NULL),
(4, 'Australian Cashmere', 1, 0, GETDATE(), NULL),
(4, 'Boer', 1, 0, GETDATE(), NULL),
(4, 'British Alpine', 1, 0, GETDATE(), NULL),
(4, 'Kashmir', 1, 0, GETDATE(), NULL),
(4, 'Kiko', 1, 0, GETDATE(), NULL),
(4, 'LaMancha', 1, 0, GETDATE(), NULL),
(4, 'Meat Goat', 1, 0, GETDATE(), NULL),
(4, 'Miniature Nigerian', 1, 0, GETDATE(), NULL),
(4, 'Myotonic', 1, 0, GETDATE(), NULL),
(4, 'Nigerian Dwarf', 1, 0, GETDATE(), NULL),
(4, 'Nubian', 1, 0, GETDATE(), NULL),
(4, 'New Zealand', 1, 0, GETDATE(), NULL),
(4, 'Oberhasli', 1, 0, GETDATE(), NULL),
(4, 'Pygmy', 1, 0, GETDATE(), NULL),
(4, 'Pygora', 1, 0, GETDATE(), NULL),
(4, 'Saanen', 1, 0, GETDATE(), NULL),
(4, 'Saanen-Alpine', 1, 0, GETDATE(), NULL),
(4, 'Sable', 1, 0, GETDATE(), NULL),
(4, 'Sable-Alpine', 1, 0, GETDATE(), NULL),
(4, 'Spanish', 1, 0, GETDATE(), NULL),
(4, 'Spanish-Nubian', 1, 0, GETDATE(), NULL),
(4, 'Swiss', 1, 0, GETDATE(), NULL),
(4, 'Toggenburg', 1, 0, GETDATE(), NULL),
(4, 'Tennessee Fainting', 1, 0, GETDATE(), NULL)

