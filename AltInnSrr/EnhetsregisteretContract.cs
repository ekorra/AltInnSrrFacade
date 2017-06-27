using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AltInnSrr.Lib
{
    [DataContract]
    public class EnhetsregisteretContract
    {
        [DataMember(Name = "organisasjonsnummer")]
        public int Organisasjonsnummer { get; set; }

        [DataMember(Name = "navn")]
        public string Navn { get; set; }

        [DataMember(Name = "stiftelsesdato")]
        public string Stiftelsesdato { get; set; }

        [DataMember(Name = "registreringsdatoEnhetsregisteret")]
        public string RegistreringsdatoEnhetsregisteret { get; set; }

        [DataMember(Name = "organisasjonsform")]
        public string Organisasjonsform { get; set; }

        [DataMember(Name = "hjemmeside")]
        public string Hjemmeside { get; set; }

        [DataMember(Name = "registrertIFrivillighetsregisteret")]
        public string RegistrertIFrivillighetsregisteret { get; set; }

        [DataMember(Name = "registrertIMvaregisteret")]
        public string RegistrertIMvaregisteret { get; set; }

        [DataMember(Name = "registrertIForetaksregisteret")]
        public string RegistrertIForetaksregisteret { get; set; }

        [DataMember(Name = "registrertIStiftelsesregisteret")]
        public string RegistrertIStiftelsesregisteret { get; set; }

        [DataMember(Name = "antallAnsatte")]
        public int AntallAnsatte { get; set; }

        [DataMember(Name = "institusjonellSektorkode")]
        public InstitusjonellSektorkode InstitusjonellSektorkode { get; set; }

        [DataMember(Name = "naeringskode1")]
        public Naeringskode1 Naeringskode1 { get; set; }

        [DataMember(Name = "postadresse")]
        public Postadresse Postadresse { get; set; }

        [DataMember(Name = "forretningsadresse")]
        public Forretningsadresse Forretningsadresse { get; set; }

        [DataMember(Name = "konkurs")]
        public string Konkurs { get; set; }

        [DataMember(Name = "underAvvikling")]
        public string UnderAvvikling { get; set; }

        [DataMember(Name = "underTvangsavviklingEllerTvangsopplosning")]
        public string UnderTvangsavviklingEllerTvangsopplosning { get; set; }

        [DataMember(Name = "overordnetEnhet")]
        public int OverordnetEnhet { get; set; }

        [DataMember(Name = "links")]
        public List<Link> Links { get; set; }
    }

    [DataContract]
    public class InstitusjonellSektorkode
    {
        [DataMember(Name = "kode")]
        public string Kode { get; set; }

        [DataMember(Name = "beskrivelse")]
        public string Beskrivelse { get; set; }
    }

    [DataContract]
    public class Naeringskode1
    {
        [DataMember(Name = "kode")]
        public string Kode { get; set; }

        [DataMember(Name = "beskrivelse")]
        public string Beskrivelse { get; set; }
    }

    [DataContract]
    public class Postadresse
    {
        [DataMember(Name = "adresse")]
        public string Adresse { get; set; }

        [DataMember(Name = "postnummer")]
        public string Postnummer { get; set; }

        [DataMember(Name = "poststed")]
        public string Poststed { get; set; }

        [DataMember(Name = "kommunenummer")]
        public string Kommunenummer { get; set; }

        [DataMember(Name = "kommune")]
        public string Kommune { get; set; }

        [DataMember(Name = "landkode")]
        public string Landkode { get; set; }

        [DataMember(Name = "land")]
        public string Land { get; set; }
    }

    [DataContract]
    public class Forretningsadresse
    {
        [DataMember(Name = "adresse")]
        public string Adresse { get; set; }

        [DataMember(Name = "postnummer")]
        public string Postnummer { get; set; }

        [DataMember(Name = "poststed")]
        public string Poststed { get; set; }

        [DataMember(Name = "kommunenummer")]
        public string Kommunenummer { get; set; }

        [DataMember(Name = "kommune")]
        public string Kommune { get; set; }

        [DataMember(Name = "landkode")]
        public string Landkode { get; set; }

        [DataMember(Name = "land")]
        public string Land { get; set; }
    }

    [DataContract]
    public class Link
    {
        [DataMember(Name = "rel")]
        public string Rel { get; set; }

        [DataMember(Name = "href")]
        public string Href { get; set; }
    }
}
