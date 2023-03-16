@echo

SET PGPASSWORD=postgres
cd /D "C:\Program Files\PostgreSQL\13\bin\"&&dropdb --host=localhost --port=5432 --username=postgres --no-password --if-exists -f cinema&&createdb --host=localhost --port=5432 --username=postgres --no-password --owner=postgres cinema&&pg_restore --host=localhost --port=5432 --username=postgres --no-password -d cinema -v "D:\_Cosas\pincha\Paqutero Vietnamita Frances con Katanas\tercer Trabajo\ultimo save\modificado en mi pc 2.backup"
