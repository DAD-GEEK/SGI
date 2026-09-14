use gestioni_ConsultorNet
go

CREATE TABLE Numerales(
	[IntNumeralID] [int] IDENTITY(1,1) NOT NULL,
	[StrCodigo] [nvarchar](50) NOT NULL,
	[StrDescripcion] [nvarchar](100) NOT NULL,
	[StrInterpretacion] [nvarchar](500) NULL,
	[IntNormaID] [int] NOT NULL,
	[BitActivo] [bit] NULL
	

	constraint PK_Numerales primary key([IntNumeralID]),
	constraint FK_Numerales_Normas foreign key([IntNormaID]) references Normas(IntNormaID),

	)

create table ElementosComunes(
	IntElementoComunID int not null identity(1,1),
	StrDescripcion varchar(200) not null,
	BitActivo bit not null
	constraint PK_ElementosComunes primary key(IntElementoComunID)
)

create table ElementosComunes_Detalle(
	IntDetalleID int not null identity(1,1),
	IntElementoComunID int not null,
	StrInterpretacion nvarchar(500) not null,
	BitActivo bit not null
	constraint PK_ElementosComunes_Detalle primary key(IntDetalleID)
	constraint FK_ElementosComunes_Detalle_ElementosComunes foreign key(IntElementoComunID) references ElementosComunes(IntElementoComunID) on delete cascade
	)

create table ElementosComunes_Normas(
	IntRegistroID int not null identity(1,1),
	IntElementoComunID int not null,
	IntNormaID int not null
	constraint PK_ElementosComunes_Normas primary key(IntRegistroID)
	constraint FK_ElementosComunes_Normas_ElementosComunes foreign key(IntElementoComunID) references ElementosComunes(IntElementoComunID) on delete cascade,
	constraint FK_ElementosComunes_Normas_Normas foreign key(IntNormaID) references Normas(IntNormaID)
)

create table ElementosComunes_Detalle_Numerales(
	IntRegistroID int not null identity(1,1),
	IntDetalleID int not null,
	IntNumeralID int not null
	constraint PK_ElementosComunes_Detalle_Numerales primary key(IntRegistroID)
	constraint FK_ElementosComunes_Detalle_Numerales_ElementosComunes_Detalle foreign key(IntDetalleID) references ElementosComunes_Detalle(IntDetalleID) on delete cascade,
	constraint FK_ElementosComunes_Detalle_Numerales_Numerales foreign key(IntNumeralID) references Numerales(IntNumeralID)
)



alter table ListasDeVerificacion_Numerales
add constraint FK_ListasDeVerificacion_Numerales_Numerales foreign key(IntNumeralID) references Numerales(IntNumeralID) on delete cascade;

alter table PlantillasListasDeVerificacion_Numerales
add constraint FK_PlantillasListasDeVerificacion_Numerales_Numerales foreign key(IntNumeralID) references Numerales(IntNumeralID) on delete cascade;

alter table Procesos_Numerales
add constraint FK_Procesos_Numerales_Numerales foreign key(IntNumeralID) references Numerales(IntNumeralID) on delete cascade;

alter table RequisitosYSoportes_Numerales
add constraint FK_RequisitosYSoportes_Numerales_Numerales foreign key(IntNumeralID) references Numerales(IntNumeralID) on delete cascade;


