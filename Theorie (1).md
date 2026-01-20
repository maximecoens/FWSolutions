# Hoofdstuk 1: REST Webservices
## 1.1 Herhaling HTTP
= HyperText Transfer Protocol

is een Client-Server protocol ~ aanvraag-antwoord protocol / request-response

- op het web
- heeft verschillende versies
### HTTP aanvraag (request)
methodes zijnde GET / POST / ...
- eerste headerlijn
- volgende headerlijnen
- lege lijn
- optionele body
![[Pasted image 20240111013619.png]]

### HTTP antwoord (response)
- eerste headerlijn
- volgende headerlijnen
- lege lijn
- optionele body
![[Pasted image 20240111013733.png]]

### Statuscodes
1xx informatief
2xx succes
3xx omleiding
4xx fout bij Client
5xx fout bij Server

###  HTTPS
Secure

maakt gebruik van SSL (Secure Sockets Layer)
standaardpoort: 443
(in tegenstelling tot 80)

## 1.2 REST webservices

Webservice: dienst aangeboden via het web
service zelf is een endpoint

REST = REpresentation State Transfer
er zijn principes om webservices te maken

### Beperkingen REST API
~ voorwaarden om aan REST webservice / API te voldoen

ezelsbrug: **SUCCCG**
iedere eerste letter stelt een voorwaarde voor, niet per se in volgorde
kan gelezen worden als "suck G"

#### Client-Server
Client & server zijn onafhankelijk (= elk op apart systeem)
Client hoeft enkel URI & API te kennen
(URI = Uniform Resource Identifier)

#### Uniforme interface
uniforme communicatie (onafhankelijk van type client)
=> maakt niet uit als men op webclient of mobiele client zit

##### Richtlijnen
- bron geörienteerd
	=> elke uri/webservice stelt een bron voor die men kan manipuleren
- expliciet gebruik v HTTP-methodes
	=> CRUD correspondeert met POST / GET / PUT / DELETE
	uri syntax richtlijnen:
		- zelfstandig naamwoord
		- geen extensies
		- kleine letters & spatie vervangen door "-"
		- querystring om te filteren
			dus NIET /user?name=robert, wél /user/robert
			niet bron zelf opvragen, wel filteren OP bron
		- vermijd Error 404 NOT FOUND
			aanspreekpunt vr gebruiker onveranderlijk houden (vr bv bladwijzers)
	
#### Statusloos
elke request staat op ZICHZELF
=> Server houdt geen info bij over gebruiker noch app context
	het is de Client die status bijhoudt

<mark style="background: #BBFABBA6;">+</mark> voordelen
- aanvraag kan w doorgegeven nr andere Servers
- **Load balancing** -> verhogen v schaalbaarheid (meerdere users -> app blijft werken)
- **failover** -> verhogen v betrouwbaarheid (door geen informatie verlies)

#### Cacheable
info bijhouden om later te hergebruiken

response bevat
- al dan niet cacheable?
- hoelang?

<mark style="background: #BBFABBA6;">+</mark> voordeel
verbetert **performantie**


#### Gelaagd systeem
Client weet niet met welke laag hij geconnecteerd is
=> interne Serverstructuur w afgeschermd v Client
![[Pasted image 20240111020639.png|300]]

#### Code op aanvraag
"code on demand"
is **optioneel**

naast JSON of XML mag Server ook **code** meesturen in body v HTTP bericht
Server stuurt nr Client

### Glory of REST
= mate waarin men voldoet aan [[#Beperkingen REST API]]
![[Pasted image 20240111021213.png|400]]

#### Level 0: moeras van pokken
HTTP enkel als transport
**1 endpoint**
![[Pasted image 20240111021312.png|400]]

#### Level 1: Bronnen
men werkt nu met bronnen (en **verschillende endpoints**)
maar in body v bericht w nog steeds bepaalde args meegegeven

![[Pasted image 20240111021442.png|400]]

#### Level 2: HTTP methoden
HTTP methoden als CRUD
statuscode in antwoord (foutcode indien iets mis ging)

al zeer dicht bij REST
![[Pasted image 20240111021555.png|400]]

#### Level 3: Hypermedia Controls
HATEOS (Hypertext As The Engine Of Application State)

**links** nr andere bronnen in antwoord
selfdocumenting protocol
![[Pasted image 20240111021716.png|400]]
men moet niet meer in reponse bericht zelf zoeken
"~ might be of interest to Client"


## 1.3 Java
TODO

## 1.4 Webservices testen
3 manieren
- [[#manueel]]
- herhaling [[#Unit Testen|JUnit (Java)]]
- WebTestClient (Java - Spring)

### manueel
via PostMan

### Unit Testen
 testen kleine stukjes code, bij voorkeur zonder afhankelijk te zijn van andere objecten
 indien toch afhankelijk -> mocks (="dummy's") 


#### Fixture & fases
**toestand** voordat men aan een test begint **vastleggen**
= "baseline state"

4 fases:
- setup (test fixtures klaarzetten)
- exercise: interageer met te onderzoeken systeem
- verify (controle expected resultaat)
- tear down (test fixture afbreken nr originele toestand)

Test suite: onafhankelijke unit tests met eenzelfde fixture

#### dummy's
relatie tss componenten

vervanging van bepaalde (afhankelijke) componenten

heeft **zelfde interface**
eenvoudigere implementatie/werking

ideaal vr [[SoftwareOntwikkeling/Theorie#Dependency injection|Inversion of Control]] (interfaces, abstract factory, ...)

#### mockup
= gegenereerde dummy klasse
implementeert interface
eenvoudige werking (bv vaste waarden teruggeven)

bv Mockito, EasyMock, JMock, PowerMock

#### Test schrijven
TODO: code & annotaties per test / klasse
@SpringBootTest
@Test

assertEquals(...,...)


@BeforeAll
@BeforeEach

tear down
@AfterEach
@AfterAll

### WebTestClient (Java - Spring)
Client die gebruikt wordt om HTTP berichten te sturen & ontvangen


annotaties:

we hebben ook Spring nodig
@ExtendWith(SpringExtension.class)

mag niet op gelijk welke poort (vermijd conflicten)
`@SpringBootTest(webEnvironment = WebEnvironment.RANDOM_PORT)`

automatie configuratie op juiste poort van WebClient
~injecteert WebTestClient
@AutoConfigureWebTestClient ( boven Test klasse)

```java
@Autowired
private WebTestClient webClient;
```

juiste inhoud/headers/statuscode?
![[Pasted image 20240111135647.png]]
`.exchange()` -> bericht sturen nr Server & wachten op antwoord

#### profiles
andere beans(componenten, services, ...) gebruiken afhankelijk van een profiel (test, prod, dev)

=> bean annoteren met een profiel
profiel toekennen aan een programma, test, ...

```java
@Component
@Profile("test")
public class DummyDao implements Dao {...}
```

ofwel allesbehalve testprofiel: 
```java
@Component
@Profile("!test")
public class RealDao implements Dao {...}
```

geen Profile = standaardprofiel (default)

afhankelijk vh profiel zal Springframework een ander type object injecteren:
`@ActiveProfiles`
```java
@SpringBootTest(webEnvironment = webEnvironment.RANDOM_PORT)
@AutoConfigureWebTestClient
@ActiveProfiles("test") // < ---
public class TestClass {...}
```

bij uitvoeren van app zelf doet men dit via een config file / variabelen / maven profiel / command option

## 1.4 beveiligen van webservices
[[#Authenticatie]]: gebruiker herkennen (login - wachtwoord)
	PasswordEncoder: wachtwoord veilig bewaren

[[#Autorisatie]]: rechten instellen

[[#CSRF]]: Cross Site Request Forgery

HTTPS gebruiken wanneer men dit deployt

### Authenticatie
gebruikers configureren
![[Pasted image 20240111140928.png]]
klasse met methode met
Bean annotatie om in andere services te kunnen gebruiken
die een aantal gebruikers bevat (`.build()`)
returnt manager

### Autorisatie
rechten bepalen
toegang beperken

![[Pasted image 20240111141249.png]]
Client stuurt bericht nr Servlet, maar bericht passeert een aantal filters die de toegang al dan niet blokkeren
P.S. (Servlets krijgen een Java klasse binnen & genereren een antwoord)

hoe te implementeren / configureren:

opnieuw in klasse SecurityConfig
![[Pasted image 20240111141509.png]]opnieuw Bean annotatie boven methode

filterChain die zegt: "gebruikers moeten aangemeld zijn op een manier". Hiervoor gebruiken we de httpBasic(...) authenticatie gebruiken

`.build()` op einde

#### op basis van pad
via `authorizeHttpRequests` & `requestMatchers`

![[Pasted image 20240111141840.png]]
bij het falen van 1 filter gaat men niet meer verder

verloop
![[Pasted image 20240111141925.png]]


andere manier om REST webservice te beveiligen:
niet de http berichten te filteren, maar wél beveiliging toe te voegen aan de methodes van de REST webservices
![[Pasted image 20240111142152.png]]prePostEnabled staat standaard op True

securedEnabled kan dmv `@Secured` annotatie beveiliging obv rollen toevoegen
```java
@Secured({"ROLVE_VIEWER", "ROLE_EDITOR"})
public void deleteUser(...){...}
```

en als jsr250 enabled is kan dit ook
```java
@RolesAllowed("ROLE_VIEWER")
public String getUsername2(){...}

@RoleAllowed({"ROLE_VIEWER", "ROLE_EDITOR"})
public boolean isValidUsername2(String username){...}
```

pre of post authenticatie verloop
![[Pasted image 20240111142506.png]]
waarbij voor of na het uitvoeren van de methode een stukje code wordt uitgevoerd

---
3e vorm van beveiliging
via `@PreAuthorize(...)` of `@PostAuthorize(...)`

```java
@PreAuthorize("hasRole('ROLE_VIEWER')")
public ...

@PreAuthorize("#username == authentication.principal.username") // <- Spring Expression language
...
```

#### op request vs methode
![[Pasted image 20240111143301.png]]
op bericht is een stuk eenvoudiger wegens 1 config klasse

op methodes: meer flexibiliteit

#### CSRF
= Cross Site Request Forgery


hoe weten als een ingevuld formulier van de juiste website komt of een "gehackte"?
1. **Synchronized Token Pattern** 
	generatie van een token bij het aanmaken van het formulier & nagaan als deze token dan ook wordt meegestuurd met de aanvraag
![[Pasted image 20240111143913.png]]

2. **SameSite-attribuut**
	cookie wordt enkel gebruikt voor specifieke site & niet op andere sites die op dat ogenblik open staan

CSFR staat standaard aan
men kan dit ook gaan uitschakelen in de SecurityConfig filterChain...
![[Pasted image 20240111144149.png]]

# Hoofdstuk 2: Object Relational Mapping (ORM)
## 2.1 ORM
mapping data <-> objecten

er is een duidelijk verschil in OO-concept & relationele db

verschillen:
- identiteit
- overerving
- relaties

schaduwinformatie = domeinmodel "besmetten" met extra info om te matchen met relationele DB
bv een primaire sleutel ~ID

### ORM in Java
JPA = Java Persistence API
we leggen mapping vast dmv annotaties

klasse moet Java Beans zijn om weggeschreven te kunnen worden nr DB
hoe?
- default Constructor
- getters & setters vr attributen

bv
```java
import javax.persistence.*;

@Entity
@Table(name="sportclub")
public class Sportclub {
	private Long id;
	private String naam;

	@Id
	@GeneratedValue(strategy=GenerationType.IDENTITY)
	public Long getId() { return id; }
	public String getNaam() { return naam; }

	//@Basic // mag, maar moet niet
	//@Column(name="NAAM") // men kan naam aanpassen
	public String getNaam() { return naam; }
	public void setNaam(String naam) { this.naam = naam; }

}
```

Entiteit:
- beheerd door JPA-framework
- klasse = tabel
optioneel `@Table(name="...")` om naam aan te passen

ID kan men op 4 meerdere manieren genereren (AUTO, IDENTITY, SEQUENCE, TABLE)

niet bewaren in tabel = `@Transient`
dit is dan bv een berekening zoals `getGemiddelde()` of dergelijke


een ORM framework moet volgende voorzien:
- basis CRUD bewerkingen
- taalqueries (gebruik makend van attributen & klassen)
bv niet enkel op ID ophalen, maar "alle personen wiens naam start met 't' "
- technieken voorzien
	- dirty checking: wat als object veranderd is in programma, maar nog niet in DB?
	- lazy association fetching: afhankelijke objecten pas ophalen wnr dit nodig is
	- ...

## 2.2 JPA in Spring
JPA is interface die aangeboden wordt aan een Java programma

pom.xml geeft springboot jpa dependency 

klassen annoteren, zie [[#ORM in Java]]
men kan annotatie ofwel boven getter, ofwel boven attribuut plaatsen
maar dit moet consistent zijn over het project heen

Stappen samengevat om JPA te gebruiken samengevat:
1. dependencies toevoegen
2. klassen annoteren
3. repository definiëren
4. (evt) DB configureren


### Repository
`JpaRepository<entiteit, type id>` extenden voor CRUD operaties
bv
```java
public interface SportclubRepository extends JpaRepository<Sportclub, Long> {}
// ander bestand
SportclubRepository repo; // geïnjecteerd
// objecten bewaren
repo.save(new Sportclub("mijn favo sportclub"));
// objecten opvragen
repo.findAll().forEach(club -> log.info(club.getNaam());
// object ophalen op ID
repo.findById(1L);
```
andere vben van methodes:
• `flush` → aanpassingen doorsturen naar de database  
• `saveAll` → meerdere entiteiten bewaren of aanpassen  
• `count` → aantal entiteiten  
• `delete`, `deleteAll` → één of meerdere entiteiten verwijderen  
• `existById` → bestaat er een entiteit met de gegeven id?


uitbreiden van Repo:
```java
public interface CustomerRepository extends JpaRepository<Customer, Long> { 
	List<Customer> findByLastName(String lastName); // lastName is eigenschap customer
}
```

configuratie db:
via `application.properties` key/value paren
```
spring.datasource.url=mysql://localhost:3306/iidb?serverTimezone=UTC
spring.datasource.username=iii
spring.datasource.password=iiipwd
```
men kan ook de DB op een specifieke manier genereren hierin:
```
spring.jpa.properties.javax.persistence.schema-generation.database.action=create  
```
= eerste keer dat db draait wordt deze aangemaakt obv structuur klasse

en er zijn ook nog scripts
```
spring.jpa.properties.javax.persistence.schema-generation.scripts.action=create  
spring.jpa.properties.javax.persistence.schema-generation.scripts.create-target=create.sql  
spring.jpa.properties.javax.persistence.schema-generation.scripts.create-source=metadata
```
genereert ook nog eens een SQL script voor de aangemaakte DB

## 2.3 overerving
dit concept is niet echt gekend in de wereld van relationele DBs
dus hoe vertalen?
! niet altijd gewoon meerdere klassen dus elke klasse = een tabel

3 mogelijkheden:
### 1. volledige structuur vertalen nr 1 tabel (dit is default)
![[Pasted image 20240111191834.png|300]]
<mark style="background: #FF5582A6;">-</mark> nadelen:
	- verplichte velden zijn niet mogelijk (want bv salaris is leeg bij klant & voorkeuren is leeg bij werknemer enz) ~er zit afwisseling in welke velden leeg gelaten worden
	- moeilijk uitbreidbaar

implementatie:
in "superklasse"
```java
@Entity
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@DiscriminatorColumn(name = "<naam kolom>",  
discriminatorType= DiscriminatorType.STRING) // String bijvoorbeeld
public abstract class A implements Serializable {...}
```
vr afgeleide klassen
```java
@Entity
@DiscriminatorValue("<waarde in die aangemaakte kolom>")
public class ... extends A {...}
```

```java
@Entity  
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)   // hoe wordt overerving vertaald
@DiscriminatorColumn(name = "PERSOON_TYPE",  
discriminatorType= DiscriminatorType.STRING)  // toevoegen nieuwe kolom
public abstract class Persoon implements Serializable { ... }

// klasse die erft
@Entity  
@DiscriminatorValue("K")  // waarde in toegevoegde nieuwe kolom 
public class Klant extends Persoon {...}

// andere klasse die erft
@Entity  
@DiscriminatorValue("W")  // nog een andere waarde vr in nieuwe kolom
public class Werknemer extends Persoon {...}

//...
```

### 2. elke klasse nr een tabel
![[Pasted image 20240111192943.png|300]]
dit is verticale mapping => als men vr 1 object info nodig heeft, moet men uit meerdere tabellen ophalen

implementatie:
in "superklasse"
```java
@Entity
@Inheritance(strategy = InheritanceType.JOINED)
@Table(name="...EN") // naam mag aangepast worden
public abstract class A implements Serializable {...}
```
vr afgeleide klassen
```java
@Entity
@Table(name="...EN") // mag
@PrimaryKeyJoinColumn(name="ID") // dit wijst nr PrimaryKey van A, men kan zelf kiezen hoe men deze kolom noemt in de afgeleide klasse (hier bv ID). Dit is dus een Foreign Key
public class ... extends A {...}
```
<mark style="background: #FF5582A6;">-</mark> nadelen:
- minder performant bij veel gegevens (uit meerdere tabellen ophalen vr 1 object)

! men stelt vaak nog bij de superklasse een extra kolom in met de naam van de afgeleide klasse waartoe de rij in de superklasse behoort zodat men niet moet zoeken in welke afgeleide klasse de foreign key nu eigenlijk zit. (use case: alle Personen opvragen)
![[Pasted image 20240111193925.png|200]]
implementatie daarvan:
extra discriminator column in superklasse
```java
@Entity  
@Inheritance(strategy = InheritanceType.JOINED)  
@Table(name="PERSONEN_EFFICIENT")  
@DiscriminatorColumn(name = "PERSOON_TYPE", discriminatorType=  
DiscriminatorType.STRING)  
public abstract class Persoon implements Serializable { ...}
```

DiscriminatorValue in afgeleide klassen
```java
@Entity  
@DiscriminatorValue("K")  
@Table(name="KLANTEN_EFFICIENT")  
@PrimaryKeyJoinColumn(name="ID")  
public class Klant extends Persoon {...}
```

### 3. elke concrete klasse nr eigen tabel
dus voor Persoon geen tabel, wel voor afgeleide klassen
![[Pasted image 20240111194340.png|300]]
horizontale mapping = alle info van 1 object zit in 1 tabel
<mark style="background: #BBFABBA6;">+</mark> voordeel:
mr 1 tabel voor een object
<mark style="background: #FF5582A6;">-</mark> nadeel:
moeilijk te **refactoren**: als men bij Persoon een wijziging doet, moet men dit bij alle afgeleide klassen (tabellen) wijzigen.

implementatie
```java
@Entity  
@Inheritance(strategy = InheritanceType.TABLE_PER_CLASS)  // <-- 
public abstract class Persoon implements Serializable {  
@Id  
@Column(name="PERSOON_ID")  
@GenericGenerator(name="generator", strategy="increment")  // binnen code zelf een teller bijhouden die waarde genereert
@GeneratedValue(generator="generator")  // generator op deze waarde toepassen
public int getId() { return id; }  
... }  
```
in afgeleide klasse niks speciaals
```java
@Entity  
public class Klant extends Persoon {...}
```
<mark style="background: #FF5582A6;">-</mark> nadeel:
gedeelde primaire sleutel over de verschillende afgeleide klassen (men mag niet per tabel een ID hebben)

## 2.4 associaties - valueobjecten
in OO-concept: referentie nr object
in DB: Foreign Key

Richting:
![[Pasted image 20240111235348.png|300]]

geassocieerde gegevens opvragen al dan niet via ORM
bv
loopen door alle personen en daar facturatiegegevens van op te vragen
vs
SELECT ... JOIN ... ON .... WHERE ...naam = "..."

**Value-entiteit** = object die enkel nuttig is / bestaat **enkel binnen een andere entiteit**
bv email (zonder een persoon heeft dit weinig betekenis)

type is geen entiteit, anders zou het een relatie zijn
zie de value-entiteit als een soort compositie binnenin een klasse

bv
![[Pasted image 20240112000736.png|300]]
zo is Adres hier een value-entiteit (Embedded) binnen sportclub

implementatie:
sportclub
```java
@Entity  
@Table(name = "sportclubs")  
public class Sportclub {  
	private Long id;  
	private String naam;  
	private Adres adres;  // <--
	...  
	//@Embedded  // hier hoeft embedded niet te staan, bij de klasse zelf wel
	public Adres getAdres() { return adres; }  
	public void setAdres(Adres adres) { this.adres = adres; }
	//...
}
```
adres
```java
@Embeddable  // <--
public class Adres {  
	private String straat;  
	private String huisnummer;  
	private Integer postcode;  
	private String gemeente;  
	
	public String getGemeente() { return gemeente; } 
}
```

default eigenschappen zonder annotatie:
![[Pasted image 20240112001209.png|500]]

### verzameling / collectie van value-objecten
value-entiteit kan ook verzameling van objecten zijn.
![[Pasted image 20240112001559.png|300]]

voorwaarden: 
- moet java collectie type zijn (Collection/Set/List/Map)
- type v item in collectie moet primitief of embeddable zijn

Annotaties:
`@ElementCollection`  -> verplicht
`@CollectionTable` -> als je naam van tabel wilt overschrijven omdat deze niet overeenkomt
`@Column`   -> als men naam v kolom wil overschrijven
`@AttributeOverrides` -> attributen v value-objecten overschrijven ~naam attribuut hoeft niet per se naam v kolom te zijn

implementatie:
```java
public class Sportclub {  
	private int id;  
	private String naam;  
	private Adres adres;  
	private Set<String> emails; // <--
	 
	@ElementCollection // aangeven dat dit een value-object is
		@CollectionTable(name = "emailadressen", joinColumns = @JoinColumn(name = "sportclub"))  // naam van tabel veranderen & joinColumn die kolom met primaire sleutels aangeeft
	@Column(name="email")  
	public Set<String> getEmails() { return emails; }
 
	// ...
}
```

2e vb
![[Pasted image 20240112002717.png|300]]
```java
public class Sportclub {  
	private int id;  
	private String naam;  
	private Adres adres;  
	private Set<String> emails;  
	private List<Subsidie> subsidies;  // <--
	
	@ElementCollection   // verplicht
	@CollectionTable(name = "subsidies", // naam embedded value table
	joinColumns = @JoinColumn(name = "sportclub"))  
	@AttributeOverrides({  
	@AttributeOverride(name = "jaar", 
	column = @Column(name = "sub_jaar")), // associeer naam vn kolom
	@AttributeOverride(name = "bedrag",  
	column = @Column(name = "sub_bedrag")),  
	@AttributeOverride(name = "instantie",  
	column = @Column(name = "sub_instantie"))  
	})  
	public Set<Subsidie> getSubsidies() { return subsidies; }
	
	// ...
}
```
## 2.5 ORM associaties: relaties
### 1-1
#### unidirectioneel
een student heeft een thesis
een thesis is gemaakt door 1 student

2 opties
![[Pasted image 20240112013614.png]]
implementatie:

beide entiteiten met eenzelfde primary key
Student
```java
@Entity  
@Table(name = "studenten") // <-- 
public class Student implements Serializable {  
	private int id;  
	private String naam;  
	
	@Id  
	@GeneratedValue(strategy = GenerationType.IDENTITY) 
	public int getId() {...}  
	//...
}
```
Thesis
```java
@Entity  
@Table(name="THESISSEN")  // <--
public class Thesis implements Serializable {  
	private int id;  
	private String titel; private Student student;  
	@Id()  
	public int getId() {...}
	
	@OneToOne()  // verplicht
	@MapsId  // gebruik primaire sleutel van geassocieerde entiteit
	@JoinColumn(name="id")  
	public Student getStudent() {...}  
	//...
}

```
1 van beide entiteiten heeft een foreign key (met unique constraint)
![[Pasted image 20240112015729.png]]
implementatie:
Student
```java
@Entity  
@Table(name = "studenten")  
public class Student implements Serializable {  
	//...  
	@Id  
	@GeneratedValue(strategy = GenerationType.IDENTITY)  
	public int getId() {...}  
	//...
}
```

```java
@Entity  
public class Masterproef implements Serializable {  
	//...  
	@Id  
	@GeneratedValue(strategy = GenerationType.IDENTITY) 
	public int getId() {...}
	  
	@OneToOne(optional=false) // verplicht, kan niet null zijn
	@JoinColumn(name="student",unique=true,nullable=false,updatable=false)  // kolom met verwijssleutel
	public Student getStudent() {...}  
	//...
}
```
#### bidirectioneel
implementatie
Student: attribuut masterproef toevoegen

vervolgens kan men
```java
List<Masterproef> maakMasterproeven(List<Student> studenten) {  
	List<Masterproef> masterproeven = new ArrayList<>();  
	String[] titels = {"Hibernate", "ORM", "Linq", "JDBC", "ADO.NET", "JSF", "JPA", "JAXB", "Webservices"};  
	for (int i = 0; i < titels.length; i++) {  
		Masterproef masterproef = new Masterproef();  
		masterproef.setTitel(titels[i]);  
		masterproef.setStudent(studenten.get(i));  
		studenten.get(i).setMasterproef(masterproef);  
		masterproeven.add(masterproef);  
	}
	return masterproeven;  
}
```

bij delen van zelfde primary key
Thesis
```java
@Id()
public int getId() {...}  
@OneToOne() // verplicht
@MapsId
@JoinColumn(name="id")  
public Student getStudent() {...}
```
Student
```java
@OneToOne(mappedBy="student") // mappedBy voor bidirectie
public Thesis getThesis(){...}
```
### 1-veel
#### unidirectioneel
een sportclub heeft meerdere leden
![[Pasted image 20240112012338.png|200]]
implementatie:
Lid
```java
@Entity  
@Table(name = "leden")  
public class Lid implements Serializable {  
	// ...  
	@Id  
	@GeneratedValue(strategy = GenerationType.IDENTITY)  
	public int getId() {...}  
	// ...  
	@ManyToOne // <--
	@JoinColumn(name="sportclub") // <--
	public Sportclub getClub() {...}
	//...
}
```
men kan ook nog `nullable` op true of vals zetten als het verplicht zou zijn
#### bidirectioneel
een sportclub heeft meerdere leden
een lid heeft een sportclub
![[Pasted image 20240112012617.png]]
implementatie:
```java
Sportclub maakSportclubMetLeden(String naam) {  
	Sportclub club = maakSportclub(naam);  
	club.setNaam(naam);  
	club.setLeden(maakLeden());  
	club.getLeden().forEach(lid -> lid.setClub(club));  
	return club;
}
```

```java
// verander van club  
Lid lid = club1.getLeden().get(index);  
lid.setClub(club2);  
club2.getLeden().add(lid);  
club1.getLeden().remove(lid);
```

Lid blijft zelfde
```java
@ManyToOne  
@JoinColumn(name="sportclub")  
public Sportclub getClub() { return club; }
```
! nu ook nog in Sportclub terugwijzen
```java
@OneToMany(mappedBy="club") // <-- club omschrijft naam eigenschap lid die relatie beschrijft (ALTIJD BIJ OneToMany)
public Set<Lid> getLeden() { return leden; }
```
### veel-veel
#### unidirectioneel
en tornooi heeft meerdere spelers
een speler heeft meerdere tornooien
![[Pasted image 20240112013242.png|300]]
implementatie:
Speler
```java
@ManyToMany(mappedBy="deelnemers") // mappedBy geeft naam v eigenschap v tornooi die relatie beschrijft  
public Set<Tornooi> getTornooien() {  
return tornooien;  
}
```

### cascade
actie op entiteit  -> actie op entiteit in relatie?

Enum `javax.persistence.Cascadetype`

voorbeelden
```java
@OneToMany(cascade=CascadeType.REMOVE, mappedBy="customer")  
public Set<CustomerOrder> getOrders() { return orders; }
```

soorten cascade
- ALL  
-  DETACH  
- MERGE  
- PERSIST  
- REFRESH  
- REMOVE
## 2.6 ORM - werking
![[Pasted image 20240112021214.png|300]]
### EntityManager
beheert entiteiten in een "persistence context"

in container
```java
@PersistenceContext  
EntityManager em;
```

in application
```java
@PersistenceUnit  
EntityManagerFactory emf;
```

```java
EntityManager em = emf.createEntityManager();
```

vb implementatie
```java
String[] clubs = {"EIKENLO", ... , "LANDEGEM BC FV"};  
EntityManagerFactory entityManagerFactory =  
Persistence.createEntityManagerFactory("BadmintonJPAPU");
EntityManager entityManager = entityManagerFactory.createEntityManager();  
entityManager.getTransaction().begin();  
for (String club : clubs) {  
	Sportclub sportclub = new Sportclub();  
	sportclub.setNaam(club);  
	entityManager.persist(sportclub);  
}  
entityManager.getTransaction().commit();  
entityManager.close();
```

### werken met objecten
#### toestanden van een object
levensloop object (beheerd door entityManager)
![[Pasted image 20240112022308.png|400]]
new (aanmaken) -> persist (komt in cache)
remove -> bij volgende transactie verwijderen
refresh -> bij nieuwe info in DB het object vernieuwen
detached -> wnr entityManager w gesloten
merge -> bestaande objecten worden gemengd met huidige toestand

##### object persistent maken
= toevoegen aan DB
`entityManager.persist(obj);`

Cascading style (geassocieerd objecten ook persisten)
vb
```java
// ...
@OneToMany(mappedBy="person", cascade=CascadeType.PERSIST)
private List<Address> addresses;
```
##### object opvragen
obv identifier
`T find(Class<T> entityClass, Object primaryKey)
(null indien onbestaand)

vb
```java
public Sportclub getSportclub(Long id) {  
	EntityManager em = emf.createEntityManager();  
	Sportclub sportclubOpId = em.find(Sportclub.class, id);  // <--
	em.close();  
	return sportclubOpId;  
}
```

het meermaals opvragen v een object met zelfde id => verschillende referenties nr zelfde object in cache

###### Lazy Fetching
geassocieerde objecten niet mee opvragen

vb
Address
```java
@Entity  
public class Address {  
	@Id  
	@GeneratedValue(strategy=...)  
	private int id;  
	private String street;  
	private int houseNumber;  
	private String city;  
	private int zipCode;  
	@ManyToOne(fetch = FetchType.LAZY) // <--  
	private Person person;  
}
```

uitwerking in andere klasse
```java
public List<Lid> getLeden(Long id) {  
	EntityManager em = emf.createEntityManager();  
	Sportclub sportclubOpId = em.find(Sportclub.class, id);  
	// lazy, ophalen en kopiëren  
	List<Lid> leden = new ArrayList< (sportclubOpId.getLeden());  
	em.close();  
	return leden;  
}
```

tegenovergestelde is EAGER (= onmiddelijk mee opvragen)
`@ManyToOne(fetch=FetchType.EAGER)`

##### object verwijderen
toestand gaat van managed -> removed
vb
```java
EntityManager em = ... ;  
Employee employee = em.find(Employee.class, 1);  
em.getTransaction().begin();  
em.remove(employee);  // <--
em.getTransaction().commit();
```

via cascading kan dit ook op associatie
`cascade=CascadeType.REMOVE`

##### object wijzigen
methode `flush` voert wijzigingen door nr DB
(dit wordt impliciet opgeroepen in `commit`)

vb
eerst vinden, dan aanpassen
```java
EntityManager em = ... ;  
Employee employee = em.find(Employee.class, 1); // <--  
em.getTransaction().begin(); // <-- 
employee.setNickname("Joe the Plumber"); // <-- 
em.getTransaction().commit(); // <--
```

detached objecten = aangemaakt door andere of niet meer bestaande entityManager 
###### merge
meegegeven obj in persistentiecontext kopiëren
enkel nodig bij doorgeven objecten tss contexten

vb
```java
EntityManager em = createEntityManager();  
Employee detached = em.find(Employee.class, id);  
em.close();  
// ...
em = createEntityManager();  
em.getTransaction().begin();  
Employee managed = em.merge(detached); // <--
em.getTransaction().commit();
```

kan opnieuw met cascade
`cascade=CascadeType.MERGE`

ander vb
```java
public void addThesissen(List<Thesis> thesissen) {  
	EntityManager em = emf.createEntityManager();  
	em.getTransaction().begin();  
	for (Thesis thesis : thesissen) {  
		Student student = em.merge(thesis.getStudent());  // <--
		thesis.setStudent(student);  // <--
		student.setThesis(thesis);  // <--
		em.persist(thesis);  // <--
	}  
	em.getTransaction().commit();  
	em.close();  
}
```

### persistence context
deel v JPA EntityManager met enkele rollen:
- dirty checken van objecten (indien niet in sync met DB) (kan geforceerd worden met `flush`)
- soort first-level cache (cachet entiteiten, meermaals opvragen -> ref nr zelfde obj)

bestaat zolang entityManager geopend is

#### obj eruit verwijderen
2 manieren om object uit persistence context te verwijderen
1. entityManager sluiten
2. expliciet `detach` oproepen
kan ook in cascade annotatie
`cascade=CascadeType.DETACH`


### Entity Locking / Concurrency
gelijktijdige toegang & integriteit bewaren

via **optimistic locking** (default)
als obj wil worden aangepast terwijl deze locked is, zal men een fout gooien. ophalen is gn probleem

of **pessimistic locking**
transactie lock op data zolang transactie loopt (dus zelfs ophalen lukt niet)
<mark style="background: #FF5582A6;">-</mark> nadeel:
vertragend

## 2.7 ORM - query

import `javax.persistence.EntityManager`
vervolg op [[#object opvragen]], maar zonder identifier
### JPQL
met gebruik van Java Persistence Query Language
lijkt op SQL

men moet telkens een zoekopdracht (query) aanmaken
vb:
alle objecten v type Sportclub ophalen & bewaren in lijst
```java
EntityManager entityManager = ... ;  
List<Sportclub> clubs = entityManager  
.createQuery("SELECT s FROM Sportclub s", Sportclub.class)  
.getResultList();
```

#### met parameters
```java
Query opdracht  
= entityManager.createQuery("SELECT l from Lid l where l.club.naam = ?1"); // <-- 
String naam = "EIKENLO";  
opdracht.setParameter(1, naam);  // <--
List<Lid> leden = opdracht.getResultList();
```
note:
`?n` telt vanaf 1
ofwel
```java
opdracht =  
entityManager.createQuery("SELECT l from Lid l where l.club.naam = :naam");  
naam = "EIKENLO";  
opdracht.setParameter("naam", naam); // "naam" is parameternaam
leden = opdracht.getResultList();
```
 note:
 hierbij dus geen tellers maar namen als parameter

in Spring repo:
```java
public interface UserRepository extends JpaRepository<User, Long> {  
	@Query("select u from User u where u.emailAddress = ?1")  // <--
	User findByEmailAddress(String emailAddress);  
}
```


#### expressies / bewerkingen

###### distinct, order by
```java
List<String> namen = entityManager  
.createQuery("select distinct s.naam from Sportclub s order by s.naam") // <-- 
.getResultList();
```

###### count, sum, min, max, avg, group by
```java
List overzicht = entityManager  
.createQuery("select s.naam, count(s) from Sportclub s group by s.naam having count(s) >= 2") // <--
.getResultList();  
System.out.println("Sportclubs met een naam die minstens 2 keer voorkomt");  
for (Object result : overzicht) {  
	Object[] temp = (Object[])result;  
	System.out.println(temp[0] + " " + temp[1]);  
}
```

###### Joins: inner join, left join, right join, full join
![[Pasted image 20240112030142.png|100]]
alle personen opvragen
```java
Query zoekopdracht  
= entityManager.createQuery("select p from Persoon");  
List personen = opdracht.getResultList();  
System.out.println("Personen");  
for (Object object : personen) {  
	Persoon persoon = (Persoon)object;  
	System.out.println(persoon.getNaam());  
}
```

### SQL-opdrachten JPA
deze keer wél sql queries
dmv `Query createNativeQuery(String sqlString, Class resultClass)`
uiteraard import `Javax.persistence.EntityManager` niet vergeten

vb
```java
List<Sportclub> sportclubs = entityManager  
.createNativeQuery("select id, naam from sportclubs", Sportclub.class)  
.getResultList();
```

met Spring repo
```java
public interface UserRepository extends JpaRepository<User, Long> {  
@Query(value = "SELECT * FROM USERS u WHERE u.status = 1", nativeQuery = true) // <--  
Collection<User> findAllActiveUsersNative();  
}
```

# Hoofdstuk 3: node.js
javascript buiten browser

serverside platform
## npm
node package manager
= pakketbeheer vr nodejs

dependencies & dergelijke opgelijst in `package.json`
hier zit ook een `scripts` key die nuttige testen/builds kan starten (start-dev, start,...)
## eenvoudige HTTP-server
## routing
![[Pasted image 20240112215534.png]]
doorsturen nr handler
## architectuur
van Nodejs:
![[Pasted image 20240112220307.png]]
men heeft Js app, deze wordt uitgevoerd in de javascript Engine. Vervolgens maakt die dan weer gebruik van het besturingssysteem die deze op een **eventqueue**  plaatst. LibUV meerbepaald (een eventbased module die werkt via een eventloop). Blocking? -> maak er een workerThread van & via een execute callback oproepen wnr deze klaar is.

van traditionele webserver (bv Spring, Java, ASP.core,...):
![[Pasted image 20240112220326.png]]
elke aanvraag wordt toegekend aan een **Threadpool** met meerdere Threads (afhankelijk vd resources). Wnr pool vol zit deze ofwel vergroten, ofwel in een wachtrij zetten, zoals C.
geen goed idee om telkens nieuwe Threads op te starten, dit kost namelijk tijd. Beter is een Thread te hergebruiken.


samenvatting verschil:
traditioneel:
**meerdere threads** ~threadpool, indien te veel requests -> in wachtrij

node.js: **1 thread** ~eventloop, men gaat ervan uit dat deze snel afgehandeld kunnen worden
![[Pasted image 20240112220743.png|300]]

wnr welk type app?
grote app, met db en veel clients -> traditioneel
info uit servernotes inlezen of IOT zaken -> nodeJS
## Express Inleiding
## middleware
![[Pasted image 20240112221144.png]]
## routing in Express
![[Pasted image 20240112222249.png]]

http server -> middleware -> routing middleware -> (error handling) -> router -> view

route handelt af welke handler moet gebruikt worden voor een bepaald pad

samenvatting
![[Pasted image 20240112225323.png]]
## views in Express

in `layout.pug` bestand
bv 
```pug
extends layout  
block content  
h1= message  
h2= error.status  
pre #{error.stack}
```

deze code wordt dan via `res.render('index', {title, 'Express' });` gegenereerd nr HTML



# Hoofdstuk 4: websockets
verbinding opzetten tss Client & Server & zo te communiceren (gebruik makend vd poorten v HTTP protocol)

websocket protocol bepaalt hoe berichten opgesteld worden & welke uitgewisseld worden

laat ook tweewegscommunicatie toe (Server kan ook zelf data sturen nr Client) op voorwaarde dat er een connectie tss beiden is

websocket API: definieert API om websockets te gebruiken


## Architectuur
soort **permanente connectie** (over internet) waar beide partijen (client-server) berichten (binair) kunnen uitwisselen

API wordt dan gebruikt om gebruik te maken van die connectie

## Protocol
Client vraagt aan Server vr connectie / **handshake**
![[Pasted image 20240113010938.png|300]]
in headers gebruikt men key om te controleren als Server op juiste manier antwoordt

connectie wordt "geupgraded" nr websocket connectie als Server accepteert

<mark style="background: #BBFABBA6;">+</mark> voordeel van **web**socket (in tegenstelling tot gwne socket)
is dat de socket **dezelfde poorten als http(80) en https(443)** gebruikt.
men ziet verschil (tss http berichten & websocket berichten) doordat het begint met `ws` of `wss:` ipv een url
### API
sockets aanmaken die luisteren nr bepaalde events op de socket (onclose, onopen & onmessage)

vb in js:
socket maken
```js
let myWebSocket = new WebSocket("ws://www.websockets.org");
```
event listeners maken
```js
// connectie geopend  
myWebSocket.addEventListener('open', function (event) {  
myWebSocket.send('Hello Server!');  
});  
// luisteren naar berichten  
myWebSocket.addEventListener('message', function (event) {  
console.log('Message from server ', event.data);  
});  
// server sluit verbinding
myWebSocket.addEventListener('close', function(event) {  
alert("Connection closed.");  
};
```
bericht verzenden en socket sluiten
```js
myWebSocket.send("Hello WebSockets!");  
myWebSocket.close();
```


### Java Spring implementatie
![[Pasted image 20240113012505.png]]
afleiden van `TextWebSocketHandler`
vervolgens methodes overschrijven die verbonden zijn met eventhandlers van websocket protocol

### voordelen
alternatief van geen websockets gebruiken: **polling** (nu en dan als Client data opvragen)

- binair protocol, hvlheid headers zijn een **pak minder** dan bij polling
- sneller nieuwe info krijgen omdat men niet telkens moet wachten op een antwoord vooraleer men een nieuwe vraag kan sturen
![[Pasted image 20240113013026.png|400]]
bij polling mr om de 100 ms
bij websockets om de 50ms

# Hoofdstuk 5: Reactive REST webservices

als men programma heeft dat grote hvlheid data verwerkt kan men ofwel:
- traditional data processing
alle data opslaan -> inladen -> verwerken
! geheugen kan soms niet groot gng zijn -> out of memory
- stream processing
gebruik maken van reactive programming
applicatie maakt pipeline die data verwerkt
data moet niet meer allemaal gelijktijdig in geheugen
data kan "beetje bij beetje" verwerkt worden
(=> betere optie bij grote hvlheid data)

### observer push pull

wnr publisher **te (snel) veel data pusht**, maar de subscriber dit niet allemaal kan verwerken loopt de buffer vol & krijgt men dataverlies.
dit heet **backpressure** (tegendruk) (=communicatie overbelasting)
het controleermechanisme die dit afhandelt wordt ook zo genoemd (de oplossing)

men kan dus ook een **pull** gebruiken waarbij de subscriber nu en dan data opvraagt van de publisher.

### principe reactive programming
![[Pasted image 20240113021551.png]]
observable levert data aan
observer zal data verwerken

observer "subscribet" op observable om data te verwerken.
De subscription beheert de stroom van data

![[Pasted image 20240113021602.png]]
1 observable kan meerdere subscribers hebben.
die observable "multicast" dit dan nr al zn subscribers tegelijkertijd (=hot observable)

cold observable = voor elke subscriber een bepaalde hvlheid data genereren


samengevat:
![[Pasted image 20240113021945.png]]
backpressure regelt volume waarin data toekomt bij subscriber

### bibliotheken
#### Reactive Streams API (Java)
Observer is hier de Subscriber
Publisher is de Observable (data provider)
communicatie tss de 2 gebeurt via Subscription
![[Pasted image 20240113022210.png]]
Subscriber subscribet op Publisher (dit maakt Subscription aan)

Subscriber vraagt vervolgens aan Subscription een bepaalde hvlheid data (die de vraag doorstuurt nr Publisher)
Publisher publiceert data nr Subscription

Subscriber kan nu de data ophalen uit Subscription

#### Reactor
2 soorten publishers:
- Flex (biedt 2 objecten van eenzelfde type aan)
- Mono (1 object van eenzelfde type)
zodat subscribers zich hierop kunnen inschrijven

deze publishers worden dan ook gebruikt in het Spring framework om Reactive webservices te maken


### sync vs async
sync: wacht op response vooraleer nieuwe request wordt gestuurd

async: meerdere requests **zonder te wachten op response**, men blokkeert de huidige Thread niet
<mark style="background: #BBFABBA6;">+</mark> voordelen:
- sneller
- minder resources (wegens niet blokkeren van Thread)
<mark style="background: #FF5582A6;">-</mark> nadeel:
complexer
#### server
![[Pasted image 20240113024150.png]]
sync server: meerdere threads, per (blocking) request een thread

async server: gebruik maken van **eventloop** en callback (die opnieuw event op eventloop zet), alle acties erop zijn non-blocking

### architectuur sync vs async webservice
Spring MVC: **sync** webservice
server met onderlinge threadpool -> dispatcher -> request mapping -> controller -> service (acties allemaal blocking)

Spring Webflux: **async** webservice
server met onderlinge eventloop -> afhandelen binnen eventloop  -> dispatcher -> functionele endpoints (altijd non-blocking)

wanneer welke?
![[Pasted image 20240113024612.png]]
simpele CRUD -> sync

heleboel data ontvange (functionele manier van programmeren) -> async
![[Pasted image 20240113024817.png]]
sync -> Servlets -> (repos) JDBC, JPA, NoSQL

async -> Netty, Reactive Streams adapter ->  (repos) Mongo, Cassandra, Redis, ...

### Spring WebFlux
#### optie 1
dmv annotaties
resultaat is Mono of Flux
! geen blocking methodes gebruiken

vb
zorg dat men een Reactive datalaag hebt
DAO (Data Access Object)
```java
@Service  
public class BoorputDAO {  
	final Random random = new Random();  
	String[] ids = {"BP1", "BP2", "BP3"};
	  
	public Flux<Boorput> geefMetingen() { // <-- merk Flux op 
		return Flux.interval(Duration.ofSeconds(1)).take(10)  
		.map(pulse -> geefMeting());  
	}

	private Boorput geefMeting() {  
		Boorput boorput = new Boorput();  
		boorput.setId(ids[random.nextInt(ids.length)]);  
		boorput.setTijdstipDebiet(LocalDateTime.now());  
		boorput.setTijdstipPeil(LocalDateTime.now());  
		boorput.setPeil(28 + 5 * random.nextDouble());  
		boorput.setDebiet(5 + 2 * random.nextDouble());  
		return boorput;  
	}  
}
```
Controller
```java
@RestController  
@RequestMapping("boorputten")  
public class BoorputController {  
	private BoorputDAO dao;  
	public BoorputController(BoorputDAO dao) {  
		this.dao = dao;  
	}  
	@GetMapping("metingen")  
	public Flux<Boorput> haalMetingen() { // <--  
		return dao.geefMetingen();  
	}  
}
```

niet blokkerende webClient
```java
System.out.println("start test");  
ClientTestWebflux client = new ClientTestWebflux(); // <--
client.testServer();  
System.out.println("na test");
```

```java
public class ClientTestWebflux {  
	public void testServer() {  
	WebClient client = WebClient.create("http://localhost:8080");  
	Flux<Boorput> boorputFlux = client.get()  
	.uri("/boorputten/metingen")  
	.retrieve()  
	.bodyToFlux(Boorput.class);  
	boorputFlux.subscribe(System.out::println);  
	System.out.println("in test webflux");  
	}  
}
```
uitvoer
![[Pasted image 20240113025459.png]]
merk hierbij op dat de `testServer()` methode "na het uitvoeren" (wanneer "na test" wordt geprint) nog zaken print uit de methode

#### optie 2: handler
Mono & request
vb
handler
```java
@Component  
public class BoorputHandler {  
	private BoorputDAO dao;  
	public BoorputHandler(BoorputDAO dao) {  
		this.dao = dao;  
	}
	public Mono<ServerResponse> metingenVoorBoorputten(ServerRequest request) { // <-- request parameter & Mono return type 
		return ServerResponse.ok().contentType(MediaType.APPLICATION_JSON)  
		.body(dao.geefMetingen(),Boorput.class);  
	}  
}
```

handler koppelen aan een bepaald pad (~routing)
```java
@Configuration  
public class BoorputRouter {  
	@Bean  
	public RouterFunction<ServerResponse> routeBoorput(BoorputHandler boorputHandler) {  
		return RouterFunctions.route(RequestPredicates.GET("/boorputten/metingenAnders")  
	.and(RequestPredicates.accept(MediaType.APPLICATION_JSON)),  
	boorputHandler::metingenVoorBoorputten);  
	}  
}
```
merk Mono op in de Bean

routerFunction is de hulpmethode
```java
public static <T extends ServerResponse> RouterFunction<T> route(  
RequestPredicate predicate,  
HandlerFunction<T> handlerFunction)
```

# Hoofdstuk 6: JDBC
= Java Database Connectivity (API)
API om toegang te krijgen tot een DB in Java

gaat enkel over relationele DBs die SQL gebruiken

## JDBC drivers -types
![[Pasted image 20240113184501.png]]
JDBC driver biedt implementatie van JDBC API voor een 4 types drivers:
- spreekt rechtstreeks met db: zuivere javadriver
- deels afhankelijke driver: spreken met API
- JDBC-ODBC brug: vertaalt JDBC nr ODBC
- middleware driver: communiceert met middleware server die op zijn beurt bestaande DBs contacteert.

## basiswerking
### verbinding aanmaken
```java
private Connection geefVerbinding() throws SQLException {  
	return DriverManager.getConnection(  
	databaseConfig.getString("URL"),  
	databaseConfig.getString("LOGIN"),  
	databaseConfig.getString("PASWOORD"));  
}
```
`database.properties`
```properties
URL = jdbc:derby://localhost:1527/boekenwinkel  
LOGIN = iii  
PASWOORD = iiipwd
```
URL in vorm `jdbc:<subprotocol>://<subname>`
subprotocol kan [ "mysql", "derby", "postgresql"]
subname is afhankelijk van driver

### select opdracht uitvoeren
```java
try (Connection conn = geefVerbinding();  
	Statement stmt = conn.createStatement()) {  
	// opdracht uitvoeren  
	ResultSet rs = stmt.executeQuery(sqlOpdrachten.getString("Q_BOEKEN"));  
	while (rs.next()) {  
		boeken.add(new Boek(  
		rs.getString(sqlOpdrachten.getString("BOEK_ISBN")),  
		rs.getString(sqlOpdrachten.getString("TITEL")),  
		rs.getDouble(sqlOpdrachten.getString("PRIJS"))));  
	}  
}
```
### overzicht statements
```java
try (  
	Connection connection = DriverManager.getConnection(...,...,... );  
	Statement stmt = conn.createStatement()  
	) {  
	// delete-, insert-, update-opdracht of DDL  
	int aantRijenAangepast = stmt.executeUpdate(...);  
	// select-opdracht  
	ResultSet rs = stmt.executeQuery(...);  
	while (rs.next()) {  
	... = rs.getXXX(kolomNaam);  
	... = rs.getXXX(kolomNummer);  
	}  
}
```

### andere opdrachten
delete, update, insert, DDL (Data Definition Language)
```java
try (Connection conn = geefVerbinding();  
	Statement stmt = conn.createStatement()) {  
	// opdracht uitvoeren  
	String opdr = “create table temp (nr numeric)”;  
	stmt.executeUpdate(opdr);  
	String insertSql = “insert into temp values (1)”;  
	String updateSql = “update temp set nr=2 where nr=1”;  
	stmt.executeUpdate(insertSql);  
	int updateCnt = stmt.executeUpdate(updateSql);  
}
```
### prepared statements
gebruik van parameters
wordt voorgecompileerd in gegevensbank
vr opdrachten die verschillende keren uitgevoerd moeten w
<mark style="background: #BBFABBA6;">+</mark> voordelen:
- reductie uitvoeringstijd
- veiligheid (gegevensconversie & SQL injectie)

voorbeeld
```java
public Boek geefBoek(String isbn) throws BoekenNietBeschikbaarExceptie {  
	Boek boek = null;  
	try {  
		try (Connection conn = geefVerbinding();  
		PreparedStatement stmt = conn.prepareStatement(sqlOpdrachten.getString("Q_BOEK"))) {  
			stmt.setString(1, isbn);  
			ResultSet rs = stmt.executeQuery();  
			if (rs.next()) {  
			boek = new Boek(  
			rs.getString(sqlOpdrachten.getString("BOEK_ISBN")),  
			rs.getString(sqlOpdrachten.getString("TITEL")),  
			rs.getDouble(sqlOpdrachten.getString("PRIJS")));  
			}  
		}  
	} catch (SQLException e) { ... }  
	return boek;  
}
```
gebruikte `sql.properties`
```properties
# boek opvragen aan de hand van isbnnummer  
Q_BOEK = select * from boeken where isbn = ?  
# kolomnamen  
BOEK_ISBN = isbn  
TITEL = titel  
PRIJS = prijs
```

### gegevensconversie
aanhalingsteken in naam bv
`D'Haese`
NIET hardcoden
`String opdracht = "select * from studenten where naam = \'" + naam + "\'";`
wél parameter gebruiken
`String opdracht = "select * from studenten where naam = ?";`

zo voorkomt men doos ook SQL injectie

### callable statement
voorbeeld
```java
String opdracht = “{call BEWAAR_BESTEL(?,?,?)}”;  
try (Connection conn = geefVerbinding();  
CallableStatement cstmt = conn.prepareCall(opdracht)) {  
	// parameters instellen  
	cstmt.setString(1,...);  
	cstmt.setString(2,...);  
	cstmt.setInt(3,...);  
	cstmt.executeUpdate();  
}
```

### callable statement

#### aanmaken
naam procedure is SELECT_BOEKEN
```java
String opdracht = “{call SELECT_BOEKEN}”;  
CallableStatement cstmt = conn.prepareCall(opdracht);
```

ander voorbeeld
```java
CallableStatement cstmt = conn.prepareCall(opdracht);  
cstmt.registerOutParameter(1, java.sql.Types.INTEGER);  
ResultSet rs = cstmt.executeQuery();  
... // boeken ophalen  
int aantalBoeken = cstmt.getInt(1)
```
#### registreer uitvoerparam
```java
CallableStatement cstmt = conn.prepareCall(opdracht);  
cstmt.registerOutParameter(1, java.sql.Types.INTEGER);  
ResultSet rs = cstmt.executeQuery();  
... // boeken ophalen  
int aantalBoeken = cstmt.getInt(1)
```
### transacties
verschillende rijen/tabellen tegelijk aanpassen

voorbeeld
```java
Connection conn = ...;  
PreparedStatement stmt = ...;  
try {  
	stmt.setString(1,email);  
	conn.setAutoCommit(false);  
	for (Bestellijn lijn : bestelling.geefBestellijnen()) {  
		stmt.setString(2,lijn.getBoek().getId());  
		stmt.setInt(3,lijn.getHoeveelheid());  
		stmt.executeUpdate();  
	}  
	conn.commit();  
} catch (SQLException e) {  
	conn.rollback();  
} finally {  
	conn.setAutoCommit(true);  
}
```

### driver vs datasource
`java.sql` -> driver
`javax.sql` -> datasource


## Datasource
beter alternatief vr JDBC-driver
![[Pasted image 20240113232623.png]]
configuratie in `application.properties`
![[Pasted image 20240113234810.png]]

drivers in `pom.xml`


# Hoofdstuk 7: ADO.NET
ADO = Activex Data Objects

wordt gebruikt om vanuit C# te communiceren met een "gegevensbron"

principes:
- niet elke wijziging in de applicatie heeft meteen te worden doorgevoerd nr de "gegevensbron".
- vlotte overgang nr XML
## 7.1 architectuur
3 "sleutelobjecten" om te communiceren met DB (die we ook al hebben gzn bij [[#Hoofdstuk 6 JDBC|JDBC]]):
- connectie object (verbinding) = Connection
- command object (opdracht uit te voeren op gegevensbank) = Command
- datareader (om data uit te lezen uit gegevensbank) = DataReader

4e key (vormt brug tss dataset & DB):
leest data in uit DataSet & koppelt dit aan DB = DataAdapter

![[Pasted image 20240114010218.png]]
dus men kan data lokaal manipuleren & pas later doorstuurt nr DB.

Rechterdeel (DataSet) + DataAdapter zijn nieuw geïntroduceerd in de ADO architectuur

## 7.2 Dataset
= verzameling van DataTable (stelt tabel voor, bestaande uit rijen & kolommen) objecten
maakt het mogelijk om lokaal data bij te houden uit een "gegevensbank"

zo is het ook makkelijker dit om te zetten nr XML
vb 
![[Pasted image 20240114025513.png|300]]
klantDS van type DataSet met als label "KlantOrders"
bestellingen is van type DataTable, met als label "bestellingen"
tabel zelf bestaat uit 3 kolommen

#### aanmaken
DataSet
```c#
DataSet gegevens = new DataSet(); // defaultconstr
DataSet klantDS = new DataSet("KlantOrders"); // met label
```

Tabel
```c#
DataTable bestellingen = klantDS.Tables.Add("Bestellingen"); // table toevoegen met label  
bestellingen = klantDS.Tables["Bestellingen"]; // opvragen op naam 
bestellingen = klantDS.Tables[0]; // opvragen op index
```

aanmaken kolommen in tabel
```c#
// bij eerste kolom houden we returnwaarde bij om later primaire sleutel in te stellen
DataColumn bestelId = bestellingen.Columns.Add("BestelID",typeof(Int32)); // naam & type (v e klasse)  
bestellingen.Columns.Add("Hoeveelheid", typeof(Int32));  
bestellingen.Columns.Add("Firma", typeof(string));
```
**primaire sleutel** (een kolom) van tabel instellen
```c#
bestellingen.PrimaryKey = new DataColumn[] {bestelId}; // is een ARRAY van datakolom objecten omdat men een samengestelde primaire sleutel kan hebben
```

#### opvullen
```c#
// nieuwe rij  
DataRow rijBestelling = bestellingen.NewRow(); // rij is hiermee nog niet toegevoegd  
// opvullen rij  
rijBestelling[bestelID] = 3145; //kolomobject 
rijBestelling["Hoeveelheid"] = 3; // naam kolom
rijBestelling[2] = "Mijn Bedrijf"; // index kolom
// rij toevoegen aan tabel
bestellingen.Rows.Add(rijBestelling);
```

## 7.3 DataProvider
implementatie  vr ADO.NET bibliotheek vr 1 specifieke gegevensbank

![[Pasted image 20240114010218.png]]
linker deel van afbeelding

## 7.4 basiswerking
JDBC vs ADO.NET
![[Pasted image 20240114175456.png]]
gelijkaardig principe:
connectie openen -> opdracht uitvoeren -> resultaat ophalen

### verschillende stappen
we gebruiken dit als voorbeeld doorheen de stappen
![[Pasted image 20240114184548.png|400]]
#### config gegevens inlezen
applicaties: `app.config`
webapps: `web.config`

in de `ConfigurationManager` halen we deze configs dan op

bv
BestellingenDAOReader
```c#
using System.Configuration;  
// ...  
String opdracht = ConfigurationManager.AppSettings["SELECT_BESTEL"];
```

`App.config`
zie <\appSettings> als key die hierboven dan gebruikt wordt
```config
<configuration>  
<appSettings>  
	<add key="SELECT_BESTEL" value="select * from Bestellingen"/>  
</appSettings>  
</configuration>
```

ConnectionStrings
BestellingenDAOReader
```c#
ConnectionStringSettings connStringSet =  
ConfigurationManager.ConnectionStrings["bestellingen"]
```
`App.config`
 als key met <\add> tags hebben typisch een providerName & connectionString
```config
<configuration>  
<connectionStrings>  
	<add name="bestellingen" providerName="System.Data.SqlClient"  
	connectionString="..."/>  
</connectionStrings>  
</configuration>
```

#### factory vr provider aanmaken
zo krijgen we objecten voor een specifieke provider

vb
in BestellingenDAOReader
```c#
using System.Data.Common;  
// ... 
ConnectionStringSettings connStringSet =  
ConfigurationManager.ConnectionStrings["bestellingen"];  
DbProviderFactory factory =  
DbProviderFactories.GetFactory(connStringSet.ProviderName);
```

#### connectie-obj aanmaken
aan factory object een nieuwe connectie aanvragen

vb
BestellingenDAOReader
```c#
DbProviderFactory factory = ... ;  
ConnectionStringSettings connStringSet = ... ;  
DbConnection connection = factory.CreateConnection();  
connection.ConnectionString = connStringSet.ConnectionString;
```
opbouw connectionstring voorbeeld
```"Data Source=(LocalDB)\MSSQLLocalDB;  
AttachDbFilename=...\klantorders.mdf;  
Integrated Security=True;  
Connect Timeout=30"
```

#### commando-obj aanmaken
vb
BestellingenDAOReader
```c#
DbConnection conn = ... ;  
// aanmaken commando-object  
DbCommand command = conn.CreateCommand(); // <-- 
// instellen SQL-opdracht  
command.CommandText =  
ConfigurationManager.AppSettings["SELECT_BESTELLINGEN"]; // command text instellen
```

#### commando-obj uitvoeren
vb
BestellingenDAOReader
```c#
using (DbConnection conn = ...) {  
	... // configuratie connectie + aanmaken commando-object  
	conn.Open(); // eerst Connectie openen  
	// commando-object uitvoeren  
	// delete-, insert-, update-opdracht of DDL  
	int aantRijenAangepast = command.ExecuteNonQuery();  
	// OFWEL  
	// select-opdracht  
	DbDataReader reader = command.ExecuteReader();  
	...  
} // Connectie sluiten
```

#### DataReader gegevens uitlezen
vb
BestellingenDAOReader
```c#
DataTable tabelBestel = ...;  
using (DbConnection conn = ...) {  
	... // configuratie connectie + aanmaken commando-object  
	conn.Open();  
	DbDataReader reader = command.ExecuteReader();  
	while (reader.Read()) { // cursor zakt 1 rij bij .Read()  
		DataRow rij = tabelBestel.NewRow(); // nieuwe rij maken
		// opvullen  
		rij[0] = reader["BestelID"];  
		rij[1] = reader[1];  
		rij[2] = reader.GetString(2);  
		tabelBestel.Rows.Add(rij); // toevoegen aan tabel
	}  
}
```
nadeel: reader[...]: geven gwn object terug
getX(...) geven concreet type terug

#### samengevat
```c#
DbProviderFactory factory = ... ;  
using (  
DbConnection conn = factory.CreateConnection()) {  
	conn.ConnectionString = ...;  
	DbCommand command = conn.CreateCommand();  
	command.CommandText = ...;  
	conn.Open(); // Connectie openen  
	// delete-, insert-, update-opdracht of DDL  
	int aantRijenAangepast = command.ExecuteNonQuery();  
	// select-opdracht
	DbDataReader reader = command.ExecuteReader();  
	while (reader.Read()) {  
		... = reader[“kolomnaam”];  
		... = reader[volgnummer];  
		... = reader.GetXXX(volgnummer);  
	}  
}
```

## 7.5 DataAdapter
soort mini-gegevensbank in het geheugen
rol: koppeling DataSet & gegevensbank
w gebruikt om DataSet op te vullen

~makkelijke manier om data object te vullen
vb
BestellingenDAOAdapter
```c#
// adapter aanmaken  
DbDataAdapter adapter = factory.CreateDataAdapter();  
// select-opdracht instellen: opvullen dataset  
DbCommand command = ...  
adapter.SelectCommand = command;  
// dataset opvullen  
DataSet klantDS = new DataSet(DS_ORDERS);  
adapter.Fill(klantDS,TABEL_BESTELLING);
```
## 7.6 Opdrachten met parameters

### SQL-opdracht met parameter
`App.config`
```config
string query = "select * from Bestellingen where Hoeveelheid > @min";
```
@min is hier de @naamParameter, afhankelijk van provider kan dit ook `:` of `?` zijn!
### parameter aanmaken & toevoegen aan commando
```c#
// aanmaken parameter  
DbParameter parameter = factory.CreateParameter();  
// eigenschappen parameter instellen  
parameter.ParameterName = MIN;  
parameter.DbType = DbType.Int32;  
// toevoegen aan commando-object  
opdracht.Parameters.Add(parameter);  
// waarde instellen  
int minimumAantal = ... ;  
opdracht.Parameters[MIN].Value = minimumAantal;
```
### gegevensconversie
aanhalingsteken in naam
	knippen & plakken zou fout geven
### SQL-injectie
meestal in webapp

voorbeeld:
```c#
String opdracht =  
"select * from studenten where studnr = " + nummer;
```
 men zou dan bv `1=1` kunnen ingeven of `1; DROP TABLE studenten` of dergelijke

VOORKOMEN:
door parameters te gebruiken

## 7.7 [[#7.5 DataAdapter|DataAdapter]]: aanpassingen doorgeven
### principe
vult DataTable op

heeft 4 command objecten
Select, Insert, Update, Delete

### Aanmaak DataAdapter
! primaire sleutels ook ophalen
```c#
DbProviderFactory factory = ...;  
DbConnection conn = ...;  
DbDataAdapter adapter = factory.CreateDataAdapter();  
adapter.SelectCommand = MaakSelectCommand(conn);  
adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;  
adapter.InsertCommand = MaakInsertCommand(factory, conn);  
adapter.UpdateCommand = MaakUpdateCommand(factory, conn);  
adapter.DeleteCommand = MaakDeleteCommand(factory, conn);
```
### Aanmaak Command-object
volgende moet ingesteld worden:
- naam
- type
- verbinding met kolom
- versie gebruik bij uitvoer
```c#
param.ParameterName = "@naam";  
param.DbType = DbType.String;

param.SourceColumn = "naam";  
param.SourceVersion = DataRowVersion.Current;
```
### Wijzigingen doorvoeren
ongedaan maken van wijzigingen kan dmv `RejectChanges` v `DataRow`


```c#
DataSet gemeenteDS;  
// ...  
adapter.Update(gemeenteDS);
```
.Update(...) voert wijzigingen in DataTable door nr gegevensbank

### Zoeken van een rij in DataSet
vb
```c#
DataSet gemeenteDS;  
...  
DataRow dr = gemeenteDS.Tables[0].Rows.Find(postcode);
```
## Transacties
verschillende gegevensbankopdrachten **samen** uitvoeren


vb:
```c#
DbProviderFactory factory = ...;  
// verbinding met gegevensbank; transactie starten  
using (DbConnection conn = ...) {  
	...  
	conn.Open();  
	DbTransaction trans = conn.BeginTransaction();  
	...  
	// commando aanmaken en toevoegen aan transactie  
	DbCommand opdracht = conn.CreateCommand();  
	opdracht.Transaction = trans;  
	opdracht.CommandText =  
	"insert into Region (RegionID, RegionDescription) VALUES (@id, @description)";
```

met params
```c#
DbParameter param = factory.CreateParameter();  
param.ParameterName = "@id";  
param.DbType = DbType.Int32;  
opdracht.Parameters.Add(param);  
param = factory.CreateParameter();  
param.ParameterName = "@description";  
param.DbType = DbType.String;  
opdracht.Parameters.Add(param);
```

commit & rollback
```c#
// opdrachten uitvoeren  
try {  
	opdracht.Parameters["@id"].Value = 100;  
	opdracht.Parameters["@description"].Value = "...";  
	opdracht.ExecuteNonQuery();  
	opdracht.Parameters["@id"].Value = 101;  
	opdracht.Parameters["@description"].Value = "...";  
	opdracht.ExecuteNonQuery();  
	trans.Commit();  
} catch(Exception e) {  
	trans.Rollback();  
}
```
# Hoofdstuk 8: Rest webservices in .NET - Open API Spec

Restful webservice met ASP.NET Core
![[Pasted image 20240114193416.png]]
Client request -> MVC
MVC Controller -> data layer -> controller -> response

## 8.1 Rest-services in .NET
voorbeeld ToDoItems
Controller
ValuesController
![[Pasted image 20240114232217.png]]
om get request te doen zou het dan bv `https://localhost:<port>/api/values` zijn, omdat values de naam vd controller is

Model
TodoItem
![[Pasted image 20240114232232.png]]
aangetoond hoe men NOT NULL kolom & default value instelt

DbContext
( houdt verandering aan objecten bij ~Change tracking )
```c#
using Microsoft.EntityFrameworkCore;

namespace VbWebAPI.Model
{
    // Houdt ToDoItems bij
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) 
            : base(options)
        {  }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
```
ToDoItems (een DbSet) is dus de verzameling van entiteiten die beheerd wordt
### items toevoegen
in een eigen Controller
![[Pasted image 20240114232721.png|400]]
### items ophalen
![[Pasted image 20240114232745.png]]

### items aanpassen
![[Pasted image 20240114232816.png|400]]
### Configuratie
in `Program.cs`
![[Pasted image 20240114233304.png]]

### Controller
#### GET
resultaten die men zal bekomen met onderstaande code:
![[Pasted image 20240114233524.png|500]]

TodoItemsController
![[Pasted image 20240114233601.png|500]]

##### GET alle items
![[Pasted image 20240114233651.png]]
##### GET 1 item
![[Pasted image 20240114233709.png|500]]

#### POST - item toevoegen
met Location header link uiteraard

resultaten die men zal bekomen met onderstaande code:
![[Pasted image 20240114233822.png|600]]
![[Pasted image 20240114233913.png]]
Location header is hier "data voor de route (URI)"
#### POST - item toevoegen
resultaten die men zal bekomen met onderstaande code:
![[Pasted image 20240114235929.png|500]]

![[Pasted image 20240114235948.png|500]]

#### DELETE - item verwijderen
resultaten die men zal bekomen met onderstaande code:
![[Pasted image 20240115000016.png]]

![[Pasted image 20240115000050.png|500]]
## 8.2 Open API Spec
### Open API Specification
formaat die de REST-webservice beschrijft
geeft een antwoord op:
- welke endpoints (paden, bv /users)
- welke methodes ondersteunen ze
- parameters (in / output)
- welke authenticatie methodes
- informatie: contact, licentie

Waarom?
beschrijving v interface is onafhankelijk van programmeertaal
snel overzicht functionaliteit
ontkoppeling API & implementatie
API first approach -> consistentere API
leesbaar vr machines -> handig vr veel tools (postman, mocks genereren, ...)

is in JSON of YAML formaat
![[Pasted image 20240115011602.png]]
Info: informatie over beschrijving, licenties, contact
servers: op welke URLs beschikbaar, berschillend voor prod/dev/...
paths: verschillende paden
components: beschrijving types objecten
security: voorziene beveiliging
tags: groeperen paden in webpagina
externalDocs: verwijzen nr externe documentatie
### Swagger
toevoegen aan project via NuGet
![[Pasted image 20240115013129.png|300]]

3 tools:
Swagger Editor (spec beschrijven)
Swagger UI: spec genereren, interactieve documentatie
Swagger Codegen: spec -> code
#### Swagger Editor
![[Pasted image 20240115013020.png]]
#### Swagger UI
genereert JSON:
= formele beschrijving REST-service
`https://localhost:44376/swagger/v1/swagger.json`
bv
```json
"openapi": "3.0.1",
"info": {"..."},
"paths": {"..."},
"components": {"..."}
```
gegenereerde JSON info
![[Pasted image 20240115013405.png|500]]

gegenereerd JSON paths
![[Pasted image 20240115013437.png|500]]
zie slides 9 t.e.m 14  (van 31) over gegenereerde info per Spec onderdeel

genereert webpagina:
leesbare beschrijving REST-service
`https://localhost:44376/swagger/index.html`

kan ook commentaar uit code gebruiken ~javadocs (als bv beschrijving per methode)
#### configuratie
zie slides 20 tem 31
![[Pasted image 20240115014034.png]]
in `Program.cs`
Swagger info toevoegen & commentaar uit methodes in code gebruiken
![[Pasted image 20240115013946.png]]
nog steeds in `Program.cs`
`app.UseSwagger()` enz toevoegen indien in development
![[Pasted image 20240115013959.png]]

##### documentatie in code
DELETE methode commentaar
![[Pasted image 20240115014201.png]]
POST methode commentaar
![[Pasted image 20240115014251.png]]
![[Pasted image 20240115014316.png]]
eerste is `<summary>`
tweede is `<remarks>`
responses onderdeel
![[Pasted image 20240115014439.png]]
eerste is van `<\response code="...">` "201" tweede is van "400"

---
men kan als laatste ook informatie uit de data attributen halen
dankzij de annotaties
![[Pasted image 20240115014628.png|500]]
gegenereerd ziet dit er dan als volgt uit
![[Pasted image 20240115014656.png]]

### OpenAPI spec vs Swagger

OpenAPI Spec: formele beschrijving v REST-webservice in JSON of YAML

Swagger: geeft **tools om**...
- een formele beschrijving op te stellen & te editen (editor)
- formele beschrijving te genereren obv code (ui)
- implementatie te genereren obv formele beschrijving (codegen)
# Hoofdstuk 9: ASP.NET Core MVC

browser request nr Controller -> Model -> DB
DB terug nr Model -> Controller -> View -> browser
![[Pasted image 20240115042702.png|400]]

## routing config

in `Program.cs`
```c#
var builder = WebApplication.CreateBuilder(args);  
// Add services to the container.  
builder.Services.AddControllersWithViews();  
...  
var app = builder.Build();  
// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())  
{  
	app.UseExceptionHandler("/Home/Error");  
	// The default HSTS value is 30 days. You may want to change this for production ...  
	app.UseHsts();
}  
app.UseHttpsRedirection();  
app.UseStaticFiles();  
app.UseRouting();  
app.UseAuthorization();  
app.MapControllerRoute(  // <--
	name: "default",  
	pattern: "{controller=Home}/{action=Index}/{id?}");  
app.Run();
```
HomeController mappen, de default methode is Index()

### meerdere endpoints
nog steeds in `Program.cs`
```c#
app.MapControllerRoute(
	name: "blog",  
	pattern: "blog/{*article}",  
	defaults: new { controller = "Blog", action = "Article" });  
app.MapControllerRoute(
	name: "default",  
	pattern: "{controller=Home}/{action=Index}/{id?}");
```
mappings zijn als volgt
`http://localhost:65568/` -> Index() van HomeController
`http://localhost:65568/Home/About` ->  About() van HomeController
`http://localhost:65568/Account/LogOn` -> Logon() van AccountController
`http://localhost:65568/Stad` -> Index() van StadController
`http://localhost:65568/blog/hotnews` -> Article() van BlogController

### controller vb
voor `http://localhost:3250/Home/Privacy`

dit is dus de Privacy() methode van de HomeController
```c#
public class HomeController : Controller {  
	public IActionResult Privacy()  
	{  
		return View();  
	}  
}
```
wordt dus doorgestuurd nr <naamMethode.cshtml>
in die geval dus Privacy.cshtml

### View vb
```c#
@foreach (var item in Model)  
{  
	<tr>  
		<td>  
			<a asp-action="Toon" asp-route-id="@item.Naam">  
			@Html.DisplayFor(modelItem => item.Naam)  
			</a>  
		</td>  
	</tr>  
}
```
asp-action is de URL Methode van controller
asp-route-id zijn de parameter(s)

HTML helpers is hier
@Html.DisplayFor(modelitem => item.Naam)
genereert een HTML-string vr de eigenschap die het resultaat is vd lambda uitdrukking

er bestaat ook nog @Html.DisplayNameFpr(model=>model.Naam)
genereert een HTML-string vr de naam vd eigenschap
#### tag helpers

deze code toont wat gegenereerd wordt
![[Pasted image 20240115052123.png]]

#### layout - sjablonen
![[Pasted image 20240115052522.png]]

verschillende in te vullen secties
![[Pasted image 20240115053001.png]]
#### Cross-site scripting (XSS)
kwaadaardige code injecteren in webpagina
![[Pasted image 20240115051702.png]]
zo kan de attacker gevoelige informatie van de victim achterhalen omdat deze nu zicht heeft op de server.

gaat vaak "undetected" omdat de code wordt uitgevoerd op het client device

hoe vermijden?
input validatie & sanitization technieken

### data van Controller nr View
#### via ViewData ~Map

in Controller:
```c#
public IActionResult Contact()  
{  
	ViewData["Message"] = "Your contact page.";  
	return View();  
}
```

.cshtml gedeelte (View)
```html
@{  
	ViewData["Title"] = "Contact";  // <--
}  
<h2>@ViewData["Title"].</h2>  
<h3>@ViewData["Message"]</h3>  
<address>  
	One Microsoft Way<br />  
	Redmond, WA 98052-6399<br />  
	<abbr title="Phone">P:</abbr>  
	425.555.0100  
</address>  
<address>  
	<strong>Support:</strong>  
	<a href="mailto:Support@example.com">Support@example.com</a><br />  
	<strong>Marketing:</strong>  
	<a href="mailto:Marketing@example.com">Marketing@example.com</a>  
</address>
```
--- 
vb gebruik ViewData

CONTROLLER
StedenController
```c#
public IActionResult Index(){  
	ViewData["Stadsnamen"] = stadFabriek.Stadsnamen;  
	return View();  
}
```

VIEW
`Index.cshtml`
```html
@{  
	ViewData["Title"] = "Steden";  
}  
<h2> Overzicht Steden </h2>  
<ul> 
	@foreach (string naam in (string[])ViewData["Stadsnamen"]) {  
	<li>@naam</li>  
	}
</ul>
```

#### via de param v methode View
(objecten vd modelklassen)
CONTROLLER
StedenController
```c#
public IActionResult Overzicht() {  
	Gemeente[] steden = stadFabriek.Steden;  
	return View(steden);
}
```

VIEW
Overzicht.cshtml
```html
@model IEnumerable<StadApp.Models.Gemeente>  
...  
@foreach (var item in Model) {  
	<tr> <td>  
		<a asp-action="Toon" asp-route-id="@item.Naam">  
		@Html.DisplayFor(modelItem => item.Naam)  
		</a>  
	</td> </tr>  
}
```
de a tag wordt dan `<a href="/Steden/Toon/Brussel">Brussel</a>`

### informatie voor controller
#### HTTP-params

##### query string
bv `/Products/Detail?id=3`

- via Request-eigenschap
in Controller methode
```c#
public IActionResult Detail() {  
	int id = Convert.ToInt32(Request["id"]); // <--
	...  
}
```

- via params in actiemethode
in Controller methode
```c#
public IActionResult Detail(int id) {  // <--
	int identificatie = id;  
	...  
}
```
#### params in URL
bv `/Products/Detail/3`
routing is dan `/{controller}/{action}/{id}`
in Controller methode
```c#
public IActionResult Detail(int id) {  
	int identificatie = id;  
	...  
}
```

ander vb
`/Steden/Toon/Brussel`
- methode Toon in StedenController
- Stad.cshtml (als View)

StadController
```c#
// GET: /Steden/Toon/id  
public IActionResult Toon(string id)  
{  
	Gemeente stad = stadFabriek.geefStad(id);  
	return View("Stad", stad); // naam cshtml bestand & model doorgeven aan View
}
```

Stad.cshtml
```html
@model StedenApp.Models.Gemeente  
@{  
	ViewData["Title"] = "Foto's " + Model.Naam;  // <-- Model.naam
}  
<h2>Foto's @Html.DisplayFor(model => model.Naam)</h2>  
@foreach (Foto foto in Model.Fotos)  
{  
	<figure>  
		<figcaption>@foto.Omschrijving</figcaption>  
		<img alt="foto" src=@foto.Url />  
	</figure>  
}
```
model kan aangesproken worden dankzij dat het meegegeven werd in de View

##### Formulier
dit gaat dan voornamelijk om POST requests

GET methode toont leeg (in te vullen) formulier
```c#
// GET: Schaak/Create/ID  
public ActionResult Create(string id)  
{  
	TornooiMetPartijen tornooi = tornooiDAO.GetTornooi(id);  
	Partij partij = new Partij();  
	partij.Tornooi = tornooi;  
	return View(partij);  
}  
// POST: Schaak/Create/ID  
[HttpPost]  
public ActionResult Create(string ID, Partij partij)  
{  
	try {  
		TornooiMetPartijen tornooi = tornooiDAO.GetTornooi(ID);  
		partij.Tornooi = tornooi;  
		tornooiDAO.AddPartij(partij);  
		return RedirectToAction("Details", new { id = tornooi.ID });  
	} catch {  
		return View(partij);  
	}  
}
```

View van dit form
```html
<form asp-action="Create">  
	<div asp-validation-summary="ModelOnly" class="text-danger"></div>  
	<div class="form-group">  
		<label asp-for="SpelerWit" class="control-label"></label>  
		<input asp-for="SpelerWit" class="form-control" />  
		<span asp-validation-for="SpelerWit" class="text-danger"></span>  
	</div>  
	...  
	<div class="form-group">  
		<label asp-for="Winnaar" class="control-label"></label>  
		<select asp-for="Winnaar" asp-items="Html.GetEnumSelectList<Winnaar>();" class="...">  
		</select>  
		<span asp-validation-for="Winnaar" class="text-danger"></span>  
	</div>  
	<div class="form-group">  
		<input type="submit" value="Bewaar" class="btn btn-primary" />  
	</div>  
</form>
```
ziet er dan als volgt uit
![[Pasted image 20240115061938.png|400]]

```html
@model SchaakApp.Models.Partij  
@{  
	ViewData["Title"] = "Create";  
}  
...  
<div>  
	<a asp-action="Details" asp-route-id="@Model.Tornooi.ID">  
	Terug naar tornooi  
	</a>  
</div>  
@section Scripts {  
	@{await Html.RenderPartialAsync("_ValidationScriptsPartial");}  
}
```

HTML hulpmethodes alternatieven
"LabelFor" "EditorFor"
```html
<div class="editor-label">
	@Html.LabelFor(model => model.SpelerWit)
</div>
<div class="editor-field">
	@Html.EditorFor(model => model.SpelerWit)
	@Html.ValidationMessageFor(model => model.SpelerWit)
</div>
```

```html
<div class="form-group">  
	<label asp-for="Title" class="col-md-2 control-label"></label>  
	<div class="col-md-10">  
		<input asp-for="Title" class="form-control" />  
		<span asp-validation-for="Title" class="text-danger" />  
	</div>  
</div>
```

### Dependency Injection
```c#
public class StedenController : Controller  
{  
	IStedenFabriek stadFabriek;  
	public StedenController(IStedenFabriek stadFabriek)  
	{  
		this.stadFabriek = stadFabriek;  
	}  
	...
}
```

in `Program.cs`
```c#
var builder = WebApplication.CreateBuilder(args);  
// Add services to the container.  
builder.Services.AddControllersWithViews();  
//services.AddTransient<IOperationTransient, Operation>();  
//services.AddScoped<IOperationScoped, Operation>();  
builder.Services.AddSingleton<StedenApp.Models.IStedenFabriek, StedenApp.Models.StedenFabriek>();  // <--
var app = builder.Build();  
// Configure the HTTP request pipeline.  
..
```
Transient: telkens een nieuw object
Singleton: 1
Scoped: 1 per request

### Validatie
iets is vereist in frontend

#### Model
```c#
public class Partij {  
	public Tornooi Tornooi { get; set; }
	
	[Display(Name = "White Player")]  
	[Required(ErrorMessage = "White player required")]  // <--
	public string SpelerWit { get; set; }  
	...  
}
```
#### View
```c#
<div class="form-group">  
	<label asp-for="SpelerWit" class="col-md-2 control-label"></label>  
	<div class="col-md-10">  
		<input asp-for="SpelerWit" class="form-control" />  
		<span asp-validation-for="SpelerWit" class="text-danger"></span>  
	</div>  
</div>
```
zie voornamelijk "asp-for" en "asp-validation-for" die de attribuutnaam in het model moeten matchen

dit is dan het resultaat (de gegenereerde HTML)
```html
<div class="form-group">  
	<label class="control-label" for="SpelerWit">Speler Wit</label>  
	<input class="form-control" type="text" data-val="true"  
		data-val-required="Witte speler vereist" id="SpelerWit"  
		name="SpelerWit" value="" />  
	<span class="text-danger field-validation-valid"  
		data-valmsg-for="SpelerWit" data-valmsg-replace="true"></span>  
</div>
```


kan ook met jQuery
```html
<div class="form-group">  
	<label class="control-label" for="SpelerWit">Speler Wit</label>  
	<input class="form-control" type="text" data-val="true"  
		data-val-required="Witte speler vereist" id="SpelerWit"  
		name="SpelerWit" value="" />  
	<span class="text-danger field-validation-valid"  
		data-valmsg-for="SpelerWit" data-valmsg-replace="true"></span>  
</div>
```
hier zijn voornamelijk "data-valmsg-for"(aanduiden validatie vr welk formulierelem) en "data-valmsg-replace" (voor error msg) van belang

! wel nog bij jQuery het volgende toevoegen voor client-side validatie
in `_ValidationScriptsPartial.cshtml`
```html
<script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>  
<script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js">  
</script>
```

#### controller
```c#
// POST: /Schaaktornooien/Create/ID  
[HttpPost]  
[ValidateAntiForgeryToken]  
public IActionResult Create(string ID,[Bind("SpelerWit,SpelerZwart,Winnaar")]Partij partij)  
{  
	TornooiMetPartijen tornooi = tornooiDAO.GetTornooi(ID);  
	partij.Tornooi = tornooi;  
	if (ModelState.IsValid)  // <--
	{  
		tornooiDAO.AddPartij(partij);  
		return RedirectToAction("Details", new { id = tornooi.ID });  
	}  
	return View(partij);  
}
```

#### mogelijke validaties
##### required

##### enumDataType
in Model (?)
```c#
[EnumDataType(typeof(Winnaar), ErrorMessage = "White, Black or Remise!!")]  
public Winnaar Winnaar { get; set; }
```

View
```c#
<div class="form-group">  
	<label asp-for="Winnaar" class="col-md-2 control-label"></label>  
	<div class="col-md-10">  
	@*<input asp-for="Winnaar" class="form-control" />*@  
	<select asp-for="Winnaar"  
		asp-items="Html.GetEnumSelectList<Winnaar>();" class="form-control">  
	</select>  
	<span asp-validation-for="Winnaar" class="text-danger"></span>  
	</div>  
</div>
```
merk hier vooral de <\select> tag op samen met `asp-items`

##### CreditCard
model
```c#
[CreditCard] public string CreditCardNumber { get; set; }
```
ofwel
```c#
[CreditCard(  
AcceptedCardTypes=CreditCardAttribute.CardType.Visa |  
CreditCardAttribute.CardType.MasterCard)]  
public string CreditCardNumber { get; set; }
```

##### overige
- EmailAddress
```c#
[EmailAddress(ErrorMessage = "Invalid Email Address")]  
public string Email { get; set; }
```
- MaxLength
- MinLength
- StringLength
```c#
[Required]  
[StringLength(1000)]  
public string Description { get; set; }
```
- Phone
```c#
[Phone]  
public string Phone { get; set; }
```
- Range
```c#
[Required]  
[Range(0, 999.99)]  
public decimal Price { get; set; }
```
- RegularExpression
```c#
[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]  
[Required]  
[StringLength(30)]  
public string Genre {get; set;}
```
#### Remote Validation
```c#
public class User  
{  
	[Remote(action: "VerifyEmail", controller: "Users")]  
	public string Email { get; set; }  
}
```
merk vooral de action naam op

UsersController
```c#
[AcceptVerbs("Get", "Post")]  
public IActionResult VerifyEmail(string email)  
{  
	if (!_userRepository.VerifyEmail(email))  
	{  
		return Json(data: $"Email {email} is already in use.");  
	}  
	return Json(data: true);  
}
```
acition naam komt hier dus van

### Overposting en CSRF

#### overposting
= te veel info posten nr Server

Bind(...) => enkel als 1 van degene uit lijst wordt meegegeven wordt een object aangemaakt
in Controller
```c#
// POST: /Schaaktornooien/Add/ID  
[HttpPost]  
[ValidateAntiForgeryToken]  
public IActionResult Add(string ID,[Bind("SpelerWit,SpelerZwart,Winnaar")]Partij partij)  // <--
{  
	TornooiMetPartijen tornooi = tornooiDAO.GetTornooi(ID);  
	partij.Tornooi = tornooi;  
	if (ModelState.IsValid)  
	{  
	tornooiDAO.AddPartij(partij);  
		return RedirectToAction("Details", new { id = tornooi.ID });  
	}  
	return View(partij);  
}
```

verduidelijking:
```c#
public class Student  
{  
public int ID { get; set; }  
public string LastName { get; set; }  
public string FirstMidName { get; set; }  
public DateTime EnrollmentDate { get; set; }  
public string Secret { get; set; } // <--

public virtual ICollection<Enrollment> Enrollments { get; set; }  
}
```
men zou de Secret bv niet willen meegeven, dus de Bind zou dan ("ID, LastName, ...")
#### CSRF
= misbruik maken van token of cookie om Client te herkennen
= Cross-Site Request Forgery

browser stuurt auth token bij elke request

stel dat gebruiker is ingelogd op bepaalde site mbv een cookie.
dan zou een hacker kunnen een link nr de gebruiker sturen die de gebruiker nr een pagina stuurt die een js script uitvoert om die cookie op te halen.

preventie:
**twee tokens** meegeven (cookie, form (gelinkt op server) )

```c#
// POST: /Schaaktornooien/Add/ID  
[HttpPost]  
[ValidateAntiForgeryToken]  // <--
public IActionResult Create(string ID, [Bind("SpelerWit,SpelerZwart,Winnaar")]Partij partij)  
{  
TornooiMetPartijen tornooi = tornooiDAO.GetTornooi(ID);  
partij.Tornooi = tornooi;  
if (ModelState.IsValid)  
{  
tornooiDAO.AddPartij(partij);  
return RedirectToAction("Details", new { id = tornooi.ID });  
}  
return View(partij);  
}
```
zorgt ervoor dat de token effectief gecheckt wordt vóór het uitvoeren

### Globalization en Localization
Globalization: apps ontwikkelen die verschillende talen & culturen ondersteunen


localization: app (die globalization ondersteunt) aanpassen vr 1 specifieke taal/cultuur

#### stappenplan
##### maak inhoud aanpasbaar
in Controller IStringLocalizer toevoegen
```c#
public class HomeController : Controller {  
	private readonly IStringLocalizer<HomeController> _localizer;  // <--
	public HomeController(IStringLocalizer<HomeController> localizer){  
		_localizer = localizer;  
	}  
	public IActionResult About()  
	{  
		//ViewData["Message"] = "Your application description page.";  
		ViewData["Message"] = _localizer["Your application description page."];  // <--
		return View();  
	}  
	...  
	}
```

in Startup.cs
```c#
builder.Services.AddLocalization(options => options.ResourcesPath =  
"Resources");  // <--
...  
// DAO  
builder.Services.AddSingleton<ITornooiDAO, SchaaktornooiDummyDao>();
```

in View IViewLocalizer
```c#
@using Microsoft.AspNetCore.Mvc.Localization   // <--
@inject IViewLocalizer Localizer  // <--
@{  
	ViewData["Title"] = "Home Page";  
}  
...  
<p>@ViewData["Message"]</p>  
...  
<address>  
	<strong>@Localizer["Support"]:</strong>  // <--
	<a href="mailto:Support@example.com">Support@example.com</a><br />  
	<strong>@Localizer["Marketing"]:</strong>  // <--
	<a href="mailto:Marketing@example.com">Marketing@example.com</a>  
</address>
```

nogmaals in Startup.cs extra localization voor Views
```c#
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");  
builder.Services.AddControllersWithViews()  // <--
.AddViewLocalization()  // <--
.AddDataAnnotationsLocalization();  // <--
// DAO  
builder.Services.AddSingleton<ITornooiDAO, SchaaktornooiDummyDao>();
```
 in Model via DataAnnotaties
 ```c#
 public class Partij {  
	public Tornooi Tornooi { get; set; }  
	[Display(Name = "White player")]  // <--
	[Required(ErrorMessage = "White player required")]  // <--
	public string SpelerWit { get; set; }  
	...  
}
```
Display & ErrorMessage eigenschappen past automatisch Localization toe indien ingesteld

ander vb
```c#
public class Partij  
{  
	public Tornooi Tornooi { get; set; }  
	[Display(Name = "White Player")]  // <--
	[Required(ErrorMessage = "White player required")]  // <--
	public string SpelerWit { get; set; }  
	[Display(Name = "Black Player")]  // <--
	[Required(ErrorMessage = "Black player required")]  // <--
	[SpelersVerschillend("SpelerWit", ErrorMessage = "White and black are different players")]  // <-- (buiten scope van cursus)
	public string SpelerZwart { get; set; }  
	[Required]  
	[EnumDataType(typeof(Winnaar), ErrorMessage = "White, Black or Remise!!")] // <-- 
	public Winnaar Winnaar { get; set; }  
}
```

#### voorzie inhoud per taal & cultuur
een map Resources met submappen Controllers / Models / Views met een .resx extensie
bv in Resources/Controllers is een `HomeController.nl-NL.resx` bestand

#### localization middleware
gebeurt standaard obv browser

in Startup.cs
```c#
public void Configure(IApplciationBuilder app, IWebHostEnvironment env){}
	var supportedCultures = new[] { "en-US", "fr" };  
	var localizationOptions  // <--
	= new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])  
	.AddSupportedCultures(supportedCultures)  
	.AddSupportedUICultures(supportedCultures);
	
	app.UseRequestLocalization(localizationOptions); // <--
	...
}
```

### Authentication en Authorization
#### Authenticatie
inloggen & gebruiker herkennen
![[Pasted image 20240115074220.png|600]]
gegenereerde configuratie (in Program.cs)
via Identity Mechanisme
```c#
builder.Services.AddDbContext<ApplicationDbContext>(options =>  
options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); // db  
builder.Services.AddDatabaseDeveloperPageExceptionFilter();  // hulppagina met excepties
builder.Services.AddDefaultIdentity<IdentityUser>(options =>  
options.SignIn.RequireConfirmedAccount = true)  // standaard identificatiesysteem
.AddEntityFrameworkStores<ApplicationDbContext>();  // voegt storage toe
builder.Services.AddControllersWithViews();
```
en ook nog
```c#
...  
app.UseAuthentication();  
app.UseAuthorization();  
...
```
#### autorisatie
rechten toekennen

rollen & policy
##### Controller
enkel toegang indien ingelogd (vr alle)
AccountController.cs
```c#
[Authorize]  // <--
public class AccountController : Controller  
{  
	[AllowAnonymous]  
	public ActionResult Login()  
	{  
	}  
	public ActionResult Logout()  
	{  
	}  
}
```

**Actie** in controller
(enkel toegankelijk indien ingelogd)
AccountController.cs
```c#
public class AccountController : Controller  
{  
	public ActionResult Login()  
	{  
	}  
	[Authorize]  
	public ActionResult Logout()  
	{  
	}  
}
```

##### rollen
in verschillende controllers
```c#
[Authorize(Roles = "Administrator")]  
public class AdministrationController : Controller  
{...}
```

```c#
[Authorize(Roles = "HRManager,Finance")]  
public class SalaryController : Controller  
{...}
```

```c#
[Authorize(Roles = "Administrator, PowerUser")]  
public class ControlPanelController : Controller  
{  
	public ActionResult SetTime()  
	{...}  
	[Authorize(Roles = "Administrator")]  
	public ActionResult ShutDown()  
	{...}  
}
```

##### policy
= voorwaarden die men op de gebruiker kan leggen
men kan bv als voorwaarde zeggen dat de gebruiker minstens x jaar oud moet zijn

vb
in Program.cs
```c#
Builder.Services.AddAuthorization(options =>  
	{  
	options.AddPolicy("RequireAdministratorRole",  // policy toevoegen met naam
	policy => policy.RequireRole("Administrator"));  // voorwaarde van policy
	});
```

```c#
[Authorize(Policy = "RequireAdministratorRole")]  
public IActionResult Shutdown()  
{  
return View();  
}
```

# Hoofdstuk 10: webapps - authenticatie
dit gaat over inloggen & registreren

## cookies
sessies


Client -> {username, passwd} -> Server

Server -> SessionID in cookie -> Client

Client -> requests user profile via auth req met session_id -> Server (compares session id with stored one)
Server -> user profile -> Client

<mark style="background: #BBFABBA6;">+</mark> voordelen:
- mr 1x inloggen
- geldigheid vn cookie kan uitgeschakeld worden

<mark style="background: #FF5582A6;">-</mark> nadelen:
- geldig vr mr 1 domein
- gevoelig vr XSS & XSRF aanvallen
- niet statusloos (info van user moet op server bewaard worden)
## tokens
Client -> {username, passwd} -> Server
Server -> JWT token (gegenereerd van user data) -> Client (saves in localstorage)
Client -> requests user profile (set jwt token in header) -> Server (validates jwt token)
Server -> user profile -> Client

<mark style="background: #BBFABBA6;">+</mark> voordelen:
- statusloos (geen info op server nodig vr validatie / generatie)
- geen domein-problemen
- info van gebruiker w bewaard in token
- niet gevoelig vr XSRF aanvallen, want in localstorage, niet cookie

<mark style="background: #FF5582A6;">-</mark> nadelen:
- geldigheid token kan niet uitgeschakeld w
- gevoelig vr [[#Cross-site scripting (XSS)]] aanvallen

## OAuth2

### basisprincipes
use case: beperkte toegang geven aan game of dergelijke dmv inloggegevens van bv FB

Concept:
![[Pasted image 20240115081721.png|400]]
auth req
auth grant
access token 

Rollen:
Resource Owner -> Client App -> A. Resource Server of B. Auth Server
#### kwetsbaarheid
kwetsbaarheid impliciet: minst veilige optie (acces token is namelijk beschikbaar via js in browser)
### rollen

### tokens

### clickjacking
transparante iframe
![[Pasted image 20240115093508.png|300]]

oplossing:
extra X_Frame_Options header


## Single Sign On (SSO)
1x inloggen voor verschillende webapps
slechts 1x security implementeren

! single point of failure

### Security Assertion Markup Language (SAML)
...

## OAuth vs SAML
![[Pasted image 20240115093751.png]]

## 2FA
auth in 2 of meerdere stappen op meerdere manieren

obv:
- iets dat gebruiker weet (ww of pin)
- iets dat gebruiker heeft (pasje)
- iets dat hij is (fingerprint)
- locatie (waar)
- tijd (wnr toegang proberen krijgen)
