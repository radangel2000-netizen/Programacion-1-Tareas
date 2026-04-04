-- ============================================================
--  BASE DE DATOS - CENTRALITA TELEFÓNICA
--  Práctica POO - Prof. Willis Polanco
-- ============================================================

-- Crear la base de datos
CREATE DATABASE IF NOT EXISTS CentralitaTelefonica;
USE CentralitaTelefonica;

-- ── Tabla principal de llamadas ──
CREATE TABLE IF NOT EXISTS Llamadas (
    id              INT           AUTO_INCREMENT PRIMARY KEY,
    tipo_llamada    VARCHAR(20)   NOT NULL,           -- 'LOCAL' o 'PROVINCIAL'
    num_origen      VARCHAR(15)   NOT NULL,
    num_destino     VARCHAR(15)   NOT NULL,
    duracion        DOUBLE        NOT NULL,           -- en segundos
    franja          INT           DEFAULT NULL,       -- solo para provinciales (1,2,3)
    costo           DOUBLE        NOT NULL,           -- en euros
    fecha_registro  DATETIME      DEFAULT NOW()
);

-- ── Vista para ver resumen de llamadas ──
CREATE VIEW ResumenLlamadas AS
SELECT
    id,
    tipo_llamada,
    num_origen,
    num_destino,
    duracion,
    franja,
    ROUND(costo, 2)     AS costo_euros,
    fecha_registro
FROM Llamadas;

-- ── Datos de prueba ──
INSERT INTO Llamadas (tipo_llamada, num_origen, num_destino, duracion, franja, costo)
VALUES
    ('LOCAL',      '600111222', '600333444', 120, NULL, 18.00),
    ('LOCAL',      '600555666', '600777888',  45, NULL,  6.75),
    ('LOCAL',      '611000111', '611222333', 200, NULL, 30.00),
    ('PROVINCIAL', '700100200', '700300400',  90,    1, 18.00),
    ('PROVINCIAL', '700500600', '700700800',  60,    2, 15.00),
    ('PROVINCIAL', '711000111', '711222333', 150,    3, 45.00),
    ('PROVINCIAL', '722111222', '722333444',  30,    1,  6.00),
    ('PROVINCIAL', '733444555', '733666777',  75,    2, 18.75);

-- ── Consultas útiles ──
-- Ver todas las llamadas:
-- SELECT * FROM ResumenLlamadas;

-- Ver total de llamadas y facturación:
-- SELECT COUNT(*) AS total_llamadas, ROUND(SUM(costo), 2) AS total_euros FROM Llamadas;

-- Ver solo llamadas locales:
-- SELECT * FROM Llamadas WHERE tipo_llamada = 'LOCAL';

-- Ver llamadas provinciales por franja:
-- SELECT franja, COUNT(*) AS cantidad, ROUND(SUM(costo),2) AS total FROM Llamadas WHERE tipo_llamada = 'PROVINCIAL' GROUP BY franja;
