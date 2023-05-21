@echo

SET PGPASSWORD=postgres
cd /D "C:\Program Files (x86)\PostgreSQL\9.1\bin\"&&dropdb --host=localhost --port=5433 --username=postgres --no-password --if-exists -f bdprueba&&createdb --host=localhost --port=5433 --username=postgres --no-password --owner=postgres bdprueba&&pg_restore --host=localhost --port=5433 --username=postgres --no-password -d bdprueba -v "D:\_Cosas\pincha\Socio_de_Lalo\v\backup1.backup"
