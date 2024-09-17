namespace MicropolisCore
{
    /// <summary>
    /// Status bits of a map tile.
    /// TODO ALLBITS should end with MASK.
    /// TODO Decide what to do with #ANIMBIT (since sim-backend may not be the
    /// optimal place to do animation).
    /// TODO How many of these bits can be derived from the displayed tile?
    /// </summary>
    public enum MapTileBits
    {
        IsPower = 0x8000, // bit 15, tile has power.
        CanConduct = 0x4000, // bit 14. tile can conduct electricity.
        CanLit = 0x2000, // bit 13, tile can be lit.
        IsBulldozable = 0x1000, // bit 12, tile is bulldozable.
        //ANIMBIT = 0x0800, // bit 11, tile is animated.
        IsCenter = 0x0400, // bit 10, tile is the center tile of the zone.

        // Mask for the bits-part of the tile
        //ALLBITS = IsCenter | IsBulldozable | CanLit | CanConduct | IsPower,
        LOMASK = 0x03ff, // Mask for the #MapTileCharacters part of the tile

        //BLBNBIT = IsBulldozable | CanLit,
        //BLBNCNBIT = IsBulldozable | CanLit | CanConduct,
        
        
        //BNCNBIT = CanLit | CanConduct,
        //ASCBIT = CanConduct | CanLit,
       //REGBIT = CanConduct | CanLit // same as BNCNBIT
    }
}