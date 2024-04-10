import pandas as pd

# Load the TXT file (assuming it's tab-separated)
file_path = r'C:\Users\miles\Documents\UnityProjects\DataStructuresProject\Assets\Resources\Data\cleaned_data.txt'
df = pd.read_csv(file_path, sep='\t')

# Remove duplicates based on all columns
df = df.drop_duplicates()

# Remove rows with missing values in specific columns (except 'endYear')
columns_to_check = ['startYear', 'runtimeMinutes', 'genres', 'averageRating']
df = df.dropna(subset=columns_to_check)

# Output the cleaned dataframe to a new TXT file
cleaned_output_file = 'Newcleaned_data.txt'
df.to_csv(cleaned_output_file, sep='\t', index=False)
