@echo

SET PGPASSWORD=postgres
cd /D "C:\Program Files\PostgreSQL\13\bin\"&&dropdb --host=localhost --port=5432 --username=postgres --no-password --if-exists -f cinema&&createdb --host=localhost --port=5432 --username=postgres --no-password --owner=postgres cinema&&pg_restore --host=localhost --port=5432 --username=postgres --no-password -d cinema -v "D:\_Cosas\pincha\Paqutero Vietnamita Frances con Katanas\Segundo trabajo\EsquemaBD_C#\BDPaquetero_EsquemaYUtilesConsola\BDPaquetero\bin\Debug\save_bd_2023_03_20--11-20-41.backup"
