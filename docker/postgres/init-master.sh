#!/bin/bash
set -e

# Configure PostgreSQL for replication
echo "Configuring PostgreSQL Master for replication..."

# Update postgresql.conf
cat >> ${PGDATA}/postgresql.conf <<EOF
wal_level = replica
max_wal_senders = 3
max_replication_slots = 3
hot_standby = on
EOF

# Update pg_hba.conf to allow replication
echo "host replication replicator 0.0.0.0/0 md5" >> ${PGDATA}/pg_hba.conf

# Create replication user
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER replicator WITH REPLICATION ENCRYPTED PASSWORD 'replicator123';
    SELECT pg_create_physical_replication_slot('replica1_slot');
    SELECT pg_create_physical_replication_slot('replica2_slot');
EOSQL

echo "PostgreSQL Master configuration completed"
