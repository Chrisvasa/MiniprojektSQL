ALTER TABLE "public"."cva_project_person" DROP CONSTRAINT "FK_cva_project_person_project_id";
ALTER TABLE "public"."cva_project_person" DROP CONSTRAINT "FK_cva_person_project_person_id";
DROP TABLE IF EXISTS "public"."cva_project";
DROP TABLE IF EXISTS "public"."cva_project_person";
DROP TABLE IF EXISTS "public"."cva_person";
CREATE TABLE "public"."cva_project" ( 
  "id" SERIAL,
  "project_name" VARCHAR(50) NOT NULL,
  CONSTRAINT "cva_project_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."cva_project_person" ( 
  "id" SERIAL,
  "project_id" INTEGER NOT NULL,
  "person_id" INTEGER NOT NULL,
  "hours" INTEGER NOT NULL,
  CONSTRAINT "cva_project_person_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."cva_person" ( 
  "id" SERIAL,
  "person_name" VARCHAR(25) NOT NULL,
  CONSTRAINT "cva_person_pkey" PRIMARY KEY ("id")
);
INSERT INTO "public"."cva_project" ("project_name") VALUES ('Testprojekt');
INSERT INTO "public"."cva_project" ("project_name") VALUES ('Frontend');
INSERT INTO "public"."cva_project" ("project_name") VALUES ('Errorhandling');
INSERT INTO "public"."cva_project" ("project_name") VALUES ('Fullstack');
INSERT INTO "public"."cva_project" ("project_name") VALUES ('Programming');
INSERT INTO "public"."cva_project_person" ("project_id", "person_id", "hours") VALUES (1, 1, 25);
INSERT INTO "public"."cva_project_person" ("project_id", "person_id", "hours") VALUES (3, 1, 60);
INSERT INTO "public"."cva_project_person" ("project_id", "person_id", "hours") VALUES (4, 1, 20);
INSERT INTO "public"."cva_project_person" ("project_id", "person_id", "hours") VALUES (4, 2, 16);
INSERT INTO "public"."cva_person" ("person_name") VALUES ('Bob');
INSERT INTO "public"."cva_person" ("person_name") VALUES ('Fibbe');
ALTER TABLE "public"."cva_project_person" ADD CONSTRAINT "FK_cva_project_person_project_id" FOREIGN KEY ("project_id") REFERENCES "public"."cva_project" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."cva_project_person" ADD CONSTRAINT "FK_cva_person_project_person_id" FOREIGN KEY ("person_id") REFERENCES "public"."cva_person" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
