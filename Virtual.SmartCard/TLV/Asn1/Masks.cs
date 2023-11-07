namespace Virtual.SmartCard.TLV.Asn1
{
    public static class Masks
    {
        public const int BYTE_MASK = 0xFF; /*11111111*/
        // TAG
        public const int CLASS_MASK = 0xC0; /*11000000*/
        public const int ENCODING_FORM_MASK = 0x20; /*00100000*/
        public const int TYPE_MASK = 0x1F; /*00011111*/
        public const int TAG_NUMAsn1_MASK = 0x7F; /*01111111*/
        public const int HAS_TAG_NUMAsn1 = 0x80; /*10000000*/

        // LENGTH
        public const int LENGTH_MASK = 0x7F; /*01111111*/
        public const int LENGTH_LONG_FORM_MASK = 0x80; /*10000000*/

        // INTEGERS
        public const int TWO_COMPLEMENTS_MASK = 0x80; /*10000000*/

        // REALS
        public const int REAL_BASE_MASK = 0x30; /*00110000*/
        public const int REAL_BASE_2 = 0x00;    /*00000000*/
        public const int REAL_BASE_4 = 0x10;    /*00010000*/
        public const int REAL_BASE_16 = 0x20;   /*00100000*/

        public const int REAL_SIGN_MASK = 0x40; /*01000000*/
        public const int REAL_BASE_10_ENCODED = 0x3F; /* 00111111 */
        public const int REAL_EXPONENT_NEXT_OCTET = 0x03; /*00000011*/
        public const int REAL_EXPONENT_MASK = 0x03; /*00000011*/

        public const int REAL_SCALING_FACTOR_MASK = 0x0C; /*00001100*/

        public const int REAL_SPECIAL_VALUE = 0x40; /*01000000*/
        public const int REAL_PLUS_INFINITY = 0x40; /*01000000*/
        public const int REAL_MINUS_INFINITY = 0x41; /*01000001*/
        public const int REAL_NOT_NUMAsn1 = 0x42; /*01000010*/

        // OID
        public const int OID_HAS_MORE = 0x80; /*10000000*/
        public const int OID_NUMAsn1_MASK = 0x7F; /*01111111*/
    }
}