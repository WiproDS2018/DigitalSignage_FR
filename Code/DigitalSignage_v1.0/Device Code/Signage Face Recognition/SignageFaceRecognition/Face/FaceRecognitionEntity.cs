using Microsoft.WindowsAzure.Storage.Table;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition.Face
{
    /// <summary>
    /// Enitity class to store faceRecogniton Data
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.Storage.Table.TableEntity" />
    class FaceRecognitionDataEntity : TableEntity
    {
        public FaceRecognitionDataEntity()
        {

        }
        public FaceRecognitionDataEntity(Microsoft.ProjectOxford.Face.Contract.Face face)
        {
            this.RowKey = face.FaceId.ToString();
            this.PartitionKey = face.FaceAttributes.Age.ToString();
            this.Age = face.FaceAttributes.Age;
            this.Gender = face.FaceAttributes.Gender;
            this.Happiness = face.FaceAttributes.Emotion.Happiness;
            this.Neutral = face.FaceAttributes.Emotion.Neutral;
            this.Surprise = face.FaceAttributes.Emotion.Surprise;
            this.Fear = face.FaceAttributes.Emotion.Fear;
            this.Anger = face.FaceAttributes.Emotion.Anger;
            this.Contempt = face.FaceAttributes.Emotion.Contempt;
            this.Disgust = face.FaceAttributes.Emotion.Disgust;
        }
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public double Age
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the anger.
        /// </summary>
        /// <value>
        /// The anger.
        /// </value>
        public double Anger
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the sadness.
        /// </summary>
        /// <value>
        /// The sadness.
        /// </value>
        public double Sadness
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the disgust.
        /// </summary>
        /// <value>
        /// The disgust.
        /// </value>
        public double Disgust
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the fear.
        /// </summary>
        /// <value>
        /// The fear.
        /// </value>
        public double Fear
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the happiness.
        /// </summary>
        /// <value>
        /// The happiness.
        /// </value>
        public double Happiness
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the neutral.
        /// </summary>
        /// <value>
        /// The neutral.
        /// </value>
        public double Neutral
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the contempt.
        /// </summary>
        /// <value>
        /// The contempt.
        /// </value>
        public double Contempt
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the surprise.
        /// </summary>
        /// <value>
        /// The surprise.
        /// </value>
        public double Surprise
        {
            get; set;
        }
    }
}
