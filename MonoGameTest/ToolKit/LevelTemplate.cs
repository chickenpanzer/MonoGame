﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Ce code source a été automatiquement généré par xsd, Version=4.6.1055.0.
// 
namespace ToolKit {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Level {
        
        private LevelVictoryConditionsVictoryCondition[][] victoryConditionsField;
        
        private LevelContent[] contentField;
        
        private LevelTilesTile[][] tilesField;
        
        private string nameField;
        
        private string rowsField;
        
        private string columnsField;
        
        private string nextLevelField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("VictoryCondition", typeof(LevelVictoryConditionsVictoryCondition), Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public LevelVictoryConditionsVictoryCondition[][] VictoryConditions {
            get {
                return this.victoryConditionsField;
            }
            set {
                this.victoryConditionsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LevelContent[] Content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Tile", typeof(LevelTilesTile), Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public LevelTilesTile[][] Tiles {
            get {
                return this.tilesField;
            }
            set {
                this.tilesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rows {
            get {
                return this.rowsField;
            }
            set {
                this.rowsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string columns {
            get {
                return this.columnsField;
            }
            set {
                this.columnsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nextLevel {
            get {
                return this.nextLevelField;
            }
            set {
                this.nextLevelField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelVictoryConditionsVictoryCondition {
        
        private string classField;
        
        private string assetNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class {
            get {
                return this.classField;
            }
            set {
                this.classField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assetName {
            get {
                return this.assetNameField;
            }
            set {
                this.assetNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelContent {
        
        private LevelContentTextureRessource[] textureRessourceField;
        
        private LevelContentSoundRessource[] soundRessourceField;
        
        private LevelContentBackgroundMusic[] backgroundMusicField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TextureRessource", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LevelContentTextureRessource[] TextureRessource {
            get {
                return this.textureRessourceField;
            }
            set {
                this.textureRessourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SoundRessource", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LevelContentSoundRessource[] SoundRessource {
            get {
                return this.soundRessourceField;
            }
            set {
                this.soundRessourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BackgroundMusic", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LevelContentBackgroundMusic[] BackgroundMusic {
            get {
                return this.backgroundMusicField;
            }
            set {
                this.backgroundMusicField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelContentTextureRessource {
        
        private string keyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelContentSoundRessource {
        
        private string keyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelContentBackgroundMusic {
        
        private string keyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelTilesTile {
        
        private LevelTilesTileLayer[] layerField;
        
        private LevelTilesTileActor[] actorField;
        
        private string posXField;
        
        private string posYField;
        
        private string isWalkableField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Layer", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LevelTilesTileLayer[] Layer {
            get {
                return this.layerField;
            }
            set {
                this.layerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Actor", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LevelTilesTileActor[] Actor {
            get {
                return this.actorField;
            }
            set {
                this.actorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string posX {
            get {
                return this.posXField;
            }
            set {
                this.posXField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string posY {
            get {
                return this.posYField;
            }
            set {
                this.posYField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isWalkable {
            get {
                return this.isWalkableField;
            }
            set {
                this.isWalkableField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelTilesTileLayer {
        
        private string assetNameField;
        
        private string depthField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assetName {
            get {
                return this.assetNameField;
            }
            set {
                this.assetNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string depth {
            get {
                return this.depthField;
            }
            set {
                this.depthField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelTilesTileActor {
        
        private LevelTilesTileActorMover[] moverField;
        
        private string classField;
        
        private string assetNameField;
        
        private string pickupSoundField;
        
        private string healthValueField;
        
        private string scoreValueField;
        
        private string lightScaleField;
        
        private string defenseValueField;
        
        private string attackValueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Mover", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LevelTilesTileActorMover[] Mover {
            get {
                return this.moverField;
            }
            set {
                this.moverField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class {
            get {
                return this.classField;
            }
            set {
                this.classField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assetName {
            get {
                return this.assetNameField;
            }
            set {
                this.assetNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pickupSound {
            get {
                return this.pickupSoundField;
            }
            set {
                this.pickupSoundField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string healthValue {
            get {
                return this.healthValueField;
            }
            set {
                this.healthValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scoreValue {
            get {
                return this.scoreValueField;
            }
            set {
                this.scoreValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lightScale {
            get {
                return this.lightScaleField;
            }
            set {
                this.lightScaleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defenseValue {
            get {
                return this.defenseValueField;
            }
            set {
                this.defenseValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attackValue {
            get {
                return this.attackValueField;
            }
            set {
                this.attackValueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class LevelTilesTileActorMover {
        
        private string classField;
        
        private string moveSpeedField;
        
        private string intervalField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class {
            get {
                return this.classField;
            }
            set {
                this.classField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moveSpeed {
            get {
                return this.moveSpeedField;
            }
            set {
                this.moveSpeedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string interval {
            get {
                return this.intervalField;
            }
            set {
                this.intervalField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class NewDataSet {
        
        private Level[] itemsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Level")]
        public Level[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
}
