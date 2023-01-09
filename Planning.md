# Introduction
EntityFramwork is nice, but awefully slow. Using an SQLLite database inserting rows into a table with 4 columns is 10 times as slow as using ADO.NET with manual INSERT statements

It also seems like SELECT performance is also pretty poor in EF.

EF's database migration and creation is a good feature though since it happens at design time and not runtime where performance is not that important.

# Dapper
Seems to be an interesting alternative, but how to use it isn't very well documented for some usecases. Furthermore although some interfaces are IEnumerables, 
they ran into an out of memory exceptions, which shows that there's a list type used somewhere, and IEnumerable isn't properly implemented.

# Goals
The general goal is to develop a low-level framework which is focused on ease of use along with a high performance. Since it's supposed to be used for SQLite only, the
feature set is supposed to be very limited. Basically only CRUD operations are of interest.

This means:
* DELETE FROM xxx where ID=1234
* INSERT INTO (a,b,c) VALUES (a,b,c)
* UPDATE ....
* SELECT

# Challenges
The biggest challenge is joined select queries. Creating such a query isn't necessarily difficult, but mapping such a query to a class possibly without using reflection
shows some challenges.
