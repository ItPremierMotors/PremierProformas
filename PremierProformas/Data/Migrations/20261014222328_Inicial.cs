using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PremierProformas.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asp_net_roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_completo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    dni = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    rtn = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    telefono = table.Column<string>(type: "text", nullable: true),
                    correo = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creado_por = table.Column<string>(type: "text", nullable: true),
                    modificado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modificado_por = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marcas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'6', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prefijo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    logo_archivo = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creado_por = table.Column<string>(type: "text", nullable: true),
                    modificado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modificado_por = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_marcas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_roles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehiculos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo_inventario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    marca_id = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    modelo = table.Column<string>(type: "text", nullable: false),
                    modelo_comercial = table.Column<string>(type: "text", nullable: true),
                    condicion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: true),
                    transmision = table.Column<string>(type: "text", nullable: true),
                    capacidad_pasajeros = table.Column<int>(type: "integer", nullable: true),
                    puertas = table.Column<int>(type: "integer", nullable: true),
                    color = table.Column<string>(type: "text", nullable: true),
                    anio = table.Column<int>(type: "integer", nullable: false),
                    vin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    chasis = table.Column<string>(type: "text", nullable: true),
                    motor = table.Column<string>(type: "text", nullable: true),
                    combustible = table.Column<string>(type: "text", nullable: true),
                    cilindraje = table.Column<string>(type: "text", nullable: true),
                    garantia = table.Column<string>(type: "text", nullable: true),
                    ubicacion = table.Column<string>(type: "text", nullable: true),
                    poliza_importacion = table.Column<string>(type: "text", nullable: true),
                    numero_duca = table.Column<string>(type: "text", nullable: true),
                    factura_externa = table.Column<string>(type: "text", nullable: true),
                    fecha_ingreso = table.Column<DateOnly>(type: "date", nullable: true),
                    estatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    comentarios = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creado_por = table.Column<string>(type: "text", nullable: true),
                    modificado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modificado_por = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehiculos", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehiculos_marcas_marca_id",
                        column: x => x.marca_id,
                        principalTable: "marcas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "correlativos_proforma",
                columns: table => new
                {
                    sucursal_id = table.Column<int>(type: "integer", nullable: false),
                    anio = table.Column<int>(type: "integer", nullable: false),
                    ultimo_numero = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_correlativos_proforma", x => new { x.sucursal_id, x.anio });
                });

            migrationBuilder.CreateTable(
                name: "proformas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numero = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    sucursal_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_emision = table.Column<DateOnly>(type: "date", nullable: false),
                    dias_vigencia = table.Column<int>(type: "integer", nullable: false),
                    dirigido_a = table.Column<string>(type: "text", nullable: false),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    vehiculo_id = table.Column<int>(type: "integer", nullable: false),
                    vendedor_id = table.Column<int>(type: "integer", nullable: false),
                    jefe_ventas_nombre = table.Column<string>(type: "text", nullable: false),
                    precio_usd = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    tasa_cambio = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false),
                    precio_con_isv = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    precio_sin_isv = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    isv = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    precio_venta = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    prima = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    saldo_financiar = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creado_por = table.Column<string>(type: "text", nullable: true),
                    modificado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modificado_por = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_proformas", x => x.id);
                    table.ForeignKey(
                        name: "fk_proformas_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_proformas_vehiculos_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "vehiculos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sucursales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: false),
                    jefe_ventas_id = table.Column<int>(type: "integer", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creado_por = table.Column<string>(type: "text", nullable: true),
                    modificado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modificado_por = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sucursales", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vendedores",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_completo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    sucursal_id = table.Column<int>(type: "integer", nullable: false),
                    usuario_id = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creado_por = table.Column<string>(type: "text", nullable: true),
                    modificado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modificado_por = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vendedores", x => x.id);
                    table.ForeignKey(
                        name: "fk_vendedores_asp_net_users_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_vendedores_sucursales_sucursal_id",
                        column: x => x.sucursal_id,
                        principalTable: "sucursales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "marcas",
                columns: new[] { "id", "activo", "creado_en", "creado_por", "logo_archivo", "modificado_en", "modificado_por", "nombre", "prefijo" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "changan.png", null, null, "CHANGAN", "CH" },
                    { 2, true, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "greatwall.png", null, null, "GREAT WALL", "GR" },
                    { 3, true, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "haval.png", null, null, "HAVAL", "HA" },
                    { 4, true, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "zxauto.png", null, null, "ZXAUTO", "ZX" },
                    { 5, true, new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "deepal.png", null, null, "DEEPAL", "DE" }
                });

            migrationBuilder.InsertData(
                table: "sucursales",
                columns: new[] { "id", "activo", "codigo", "creado_en", "creado_por", "direccion", "jefe_ventas_id", "modificado_en", "modificado_por", "nombre", "telefono" },
                values: new object[,]
                {
                    { 1, true, "SPS", new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "Col. Las Mesetas, 21 calle, 17 ave. Dos cuadras arriba de CEMESA", null, null, null, "San Pedro Sula", "2556-5019" },
                    { 2, true, "TGU", new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "", null, null, null, "Tegucigalpa", "" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "asp_net_role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "asp_net_roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "asp_net_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "asp_net_user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "asp_net_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "asp_net_users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "asp_net_users",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clientes_dni",
                table: "clientes",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_marcas_nombre",
                table: "marcas",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_proformas_cliente_id",
                table: "proformas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_proformas_numero",
                table: "proformas",
                column: "numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_proformas_sucursal_id",
                table: "proformas",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "ix_proformas_vehiculo_id",
                table: "proformas",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "ix_proformas_vendedor_id",
                table: "proformas",
                column: "vendedor_id");

            migrationBuilder.CreateIndex(
                name: "ix_sucursales_codigo",
                table: "sucursales",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sucursales_jefe_ventas_id",
                table: "sucursales",
                column: "jefe_ventas_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculos_codigo_inventario",
                table: "vehiculos",
                column: "codigo_inventario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehiculos_marca_id",
                table: "vehiculos",
                column: "marca_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculos_vin",
                table: "vehiculos",
                column: "vin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vendedores_sucursal_id",
                table: "vendedores",
                column: "sucursal_id");

            migrationBuilder.CreateIndex(
                name: "ix_vendedores_usuario_id",
                table: "vendedores",
                column: "usuario_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_correlativos_proforma_sucursales_sucursal_id",
                table: "correlativos_proforma",
                column: "sucursal_id",
                principalTable: "sucursales",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_proformas_sucursales_sucursal_id",
                table: "proformas",
                column: "sucursal_id",
                principalTable: "sucursales",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_proformas_vendedores_vendedor_id",
                table: "proformas",
                column: "vendedor_id",
                principalTable: "vendedores",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sucursales_vendedores_jefe_ventas_id",
                table: "sucursales",
                column: "jefe_ventas_id",
                principalTable: "vendedores",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vendedores_asp_net_users_usuario_id",
                table: "vendedores");

            migrationBuilder.DropForeignKey(
                name: "fk_vendedores_sucursales_sucursal_id",
                table: "vendedores");

            migrationBuilder.DropTable(
                name: "asp_net_role_claims");

            migrationBuilder.DropTable(
                name: "asp_net_user_claims");

            migrationBuilder.DropTable(
                name: "asp_net_user_logins");

            migrationBuilder.DropTable(
                name: "asp_net_user_roles");

            migrationBuilder.DropTable(
                name: "asp_net_user_tokens");

            migrationBuilder.DropTable(
                name: "correlativos_proforma");

            migrationBuilder.DropTable(
                name: "proformas");

            migrationBuilder.DropTable(
                name: "asp_net_roles");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "vehiculos");

            migrationBuilder.DropTable(
                name: "marcas");

            migrationBuilder.DropTable(
                name: "asp_net_users");

            migrationBuilder.DropTable(
                name: "sucursales");

            migrationBuilder.DropTable(
                name: "vendedores");
        }
    }
}
