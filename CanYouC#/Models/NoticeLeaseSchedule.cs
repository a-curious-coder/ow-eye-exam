using System.Text.RegularExpressions;

namespace CanYouC_.Models
{
    public class NoticeLeaseSchedule
    {
        /// <summary>
        /// Entry Number of Entry
        /// </summary>
        public int EntryNumber { get; set; } = 0;

        /// <summary>
        /// Date the Entry was added
        /// </summary>
        public DateOnly EntryDate { get; set; } = new DateOnly();

        /// <summary>
        /// Date the Entry registers and where it referred to on the Title Plan
        /// </summary>    
        public string RegistrationDateAndPlanRef { get; set; } = string.Empty;

        /// <summary>
        /// A brief description of the property
        /// </summary>
        public string PropertyDescription { get; set; } = string.Empty;

        /// <summary>
        /// Date the Lease was created from and how long it will live for.
        /// </summary>
        public string DateOfLeaseAndTerm { get; set; } = string.Empty;

        /// <summary>
        /// Title number of the Lessee
        /// </summary>
        public string LesseesTitle { get; set; } = string.Empty;

        /// <summary>
        /// All appended Notes to the Entry.
        /// </summary>
        public List<string> Notes { get; set; } = new List<string>();

        public void Parse(List<string> entryText)
        {
            int latestChangeIndex = 0;
            // Every element of the entryText will contain the data we need until the Notes section.
            // Each element in the list will be a string of which we can extract the first group of characters before multiple spaces.
            for (int i = 0; i < entryText.Count; i++)
            {
                // If the entryText starts with "NOTE", then add the entryText to the Notes list and continue.
                if (entryText[i].StartsWith("NOTE"))
                {
                    Notes.Add(entryText[i]);
                    continue;
                }
                // Convert the entryText to an array of strings
                string[] rawParts = split(entryText[i]).ToArray();

                // Trim all elements in rawParts to remove excess whitespace
                for (int j = 0; j < rawParts.Length; j++)
                {
                    rawParts[j] = rawParts[j].Trim();
                }

                int validElements = 0;
                // If there's only one valid element in rawParts that's valid, then move the element to index latestChangeIndex
                for(int j = 0; j < rawParts.Length; j++)
                {
                    bool valid = rawParts[j] != "";
                    if (valid)
                    {
                        validElements++;
                        continue;
                    }
                }

                if(validElements == 1)
                {
                    rawParts[latestChangeIndex] = rawParts[0];
                    rawParts[0] = "";
                }

                // Get the index of the last element that isn't empty (AKA the latest change)
                for (int j = 0; j < rawParts.Length; j++)
                {
                    if (rawParts[j] != "")
                        latestChangeIndex = j;
                }

                RegistrationDateAndPlanRef += rawParts[0] + " ";
                PropertyDescription += rawParts[1] + " ";
                DateOfLeaseAndTerm += rawParts[2] + " ";

                // TGL513556
                // Check if rawParts[3] follows the format of 3 characters and 5-6 numbers to validate LesseesTitle
                if (Regex.IsMatch(rawParts[3], @"^[A-Z]{3}[0-9]{5,6}$"))
                {
                    LesseesTitle += rawParts[3] + "";
                
            }

            // Trim each value
            RegistrationDateAndPlanRef = RegistrationDateAndPlanRef.Trim();
            PropertyDescription = PropertyDescription.Trim();
            DateOfLeaseAndTerm = DateOfLeaseAndTerm.Trim();
            LesseesTitle = LesseesTitle.Trim();
        }

        /// <summary>
        /// Custom entryText splitter.
        /// </summary>
        /// <param name="entryText">Raw Schedule Entry Text String</param>
        /// <returns>Array of parts to extract data from for NoticeLease object variables</returns>
        public string[] split(string entryText)
        {
            string[] splitData = new string[] { "", "", "", "" };
            // NOTE: I initially thought this was about spacing. However, I noticed each group of text within any given string always started at the same point bar NOTES
            //string[] splitData = Regex.Split(contents, @"\s{5,7}").Select(s => s == "" ? null : s).ToArray();

            // NOTE: The solution I've come up with relies on an arbitrary number representing spaces that can be filled with chars.
            // TODO: Find a better solution.

            if(entryText.Length > 0)
            { 
                int firstPart = entryText.Length > 15 ? 16 : entryText.Length;
                // Extract first 16 chars.
                string first16 = entryText[..firstPart];
                splitData[0] = first16;
            }
            if(entryText.Length >= 45)
            {
                // Extract 30 chars from the 16th char.
                string next30 = entryText[16..46];
                splitData[1] = next30;
            }
            if(entryText.Length >= 61)
            {
                // Extract 16 chars from the 46th char.
                string next16 = entryText[46..62];
                splitData[2] = next16;
            }
            if(entryText.Length >= 72)
            {
                // Extract 12 chars from the 62nd char.
                string next12 = entryText[62..73];
                splitData[3] = next12;
            }

            return splitData.ToArray();
        }
    }
}
