// ייבוא ספריות React ורכיבי עיצוב מ-Material UI
import * as React from 'react';
import Box from '@mui/material/Box';
import Stepper from '@mui/material/Stepper';
import Step from '@mui/material/Step';
import StepLabel from '@mui/material/StepLabel';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import FormLabel from '@mui/material/FormLabel';
import RadioGroup from '@mui/material/RadioGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Radio from '@mui/material/Radio';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import MealsDisplay from './MealsDisplay'; // רכיב להצגת תפריט בהתאם למידע שהוזן

// רשימת שלבים לתצוגת ה-Stepper
const steps = ['הכנס פרטים אישיים', 'בחר מטרה', 'הצגת התפריט'];

/**
 * קומפוננטת Stepper לניהול שלבים עבור הצגת פרטי המשתמש
 * ומטרות התזונה.
 * 
 * @returns {JSX.Element} - קומפוננטת ה-Stepper
 */
export default function HorizontalLinearStepper() {
  // שימוש ב-useState לניהול ערכים של הטופס
  const [activeStep, setActiveStep] = React.useState(0); // שלב נוכחי
  const [weight, setWeight] = React.useState('');
  const [height, setHeight] = React.useState('');
  const [age, setAge] = React.useState('');
  const [activityLevel, setActivityLevel] = React.useState('');
  const [gender, setGender] = React.useState(null);
  const [goal, setGoal] = React.useState('maintain');

  // סטייטים לשמירת הודעות שגיאה עבור כל שדה
  const [weightError, setWeightError] = React.useState('');
  const [heightError, setHeightError] = React.useState('');
  const [ageError, setAgeError] = React.useState('');
  const [genderError, setGenderError] = React.useState('');
  const [activityLevelError, setActivityLevelError] = React.useState('');
  const [goalError, setGoalError] = React.useState('');

  /**
   * פונקציה לניהול לחיצה על כפתור "הבא"
   * בודקת אם כל השדות מולאו כראוי ומעבר לשלב הבא.
   */
  const handleNext = () => {
    let hasError = false;

    // שלב ראשון: וידוא שכל השדות מולאו
    if (activeStep === 0) {
      if (weight === '' || height === '' || age === '' || !gender || activityLevel === '' || !goal) {
        hasError = true;
        if (weight === '') setWeightError('חובה למלא');
        if (height === '') setHeightError('חובה למלא');
        if (age === '') setAgeError('חובה למלא');
        if (!gender) setGenderError('חובה לבחור מין');
        if (activityLevel === '') setActivityLevelError('חובה לבחור רמת פעילות');
        if (!goal) setGoalError('חובה לבחור מטרה');
      }
    }

    // שלב שני: בדיקה אם המטרה לא נבחרה
    if (activeStep === 1 && !goal) {
      hasError = true;
    }

    if (hasError) return;   

    // מעבר לשלב הבא
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  /**
   * פונקציה לניהול חזרה שלב אחורה
   */
  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  /**
   * פונקציה לחישוב קלוריות
   * תוכל להוסיף את החישוב על פי הנתונים שהוזנו
   */
  const calculateCalories = (weight, height, age, activityLevel, gender, goal) => {    
    // נוסחה לחישוב קלוריות בהתאם למידע שהוזן
  };

  return (
    <Box sx={{ width: '100%', textAlign: 'right' }}>
      {/* תצוגת שלבי ה-Stepper */}
      <Stepper activeStep={activeStep}>
        {steps.map((label) => (
          <Step key={label}>
            <StepLabel>{label}</StepLabel>
          </Step>
        ))}
      </Stepper>

      {/* שלב הסיום – הצגת התפריט */}
      {activeStep === steps.length ? (
        <React.Fragment>
          <Typography sx={{ mt: 2, mb: 1 }}></Typography>
          {/* תצוגת תפריט בהתאמה לפרטי המשתמש */}
          <MealsDisplay  
            age={age}
            height={height}
            weight={weight}
            isman={gender}
            lifestyle={activityLevel}
            purpose={goal}
          />
        </React.Fragment>
      ) : (
        <React.Fragment>
          {/* שלב 0 – פרטים אישיים */}
          {activeStep === 0 && (
            <Box sx={{ 
              width: '100%', 
              display: 'flex', 
              justifyContent: 'center',  // מרכז את התוכן אופקית
              alignItems: 'center',      // מרכז את התוכן אנכית
              flexDirection: 'column',   // מרכז את התוכן באופן אנכי (במקרה של כמה שדות)
              textAlign: 'right'         // שים לב שכאן נשארת אפשרות ליישר את הטקסט לימין
            }}>
              <FormControl fullWidth margin="normal">
                <FormLabel>הזן מין</FormLabel>
                <RadioGroup
                  value={gender || ''}
                  onChange={(e) => setGender(e.target.value)}
                  sx={{ textAlign: 'left' }}
                >
                  <FormControlLabel value="true" control={<Radio />} label="זכר" />
                  <FormControlLabel value="false" control={<Radio />} label="נקבה" />
                </RadioGroup>
              </FormControl>

              {/* שדה: משקל */}
              <TextField
                label="(בק''ג) משקל"
                value={weight}
                onChange={(e) => setWeight(e.target.value)}
                onBlur={() => {
                  if (weight === '' || (weight <= 30 || weight >= 300)) {
                    setWeight('');
                    if (weight === '') setWeightError('חובה למלא');
                    else if (weight > 300) setWeightError('המשקל לא יכול להיות גדול מ-300');
                    else setWeightError('המשקל לא יכול להיות קטן מ-30');
                  } else {
                    setWeightError('');
                  }
                }}
                type="number"
                fullWidth
                margin="normal"
              />
              {weightError && <Typography color="error">{weightError}</Typography>}

              {/* שדה: גובה */}
              <TextField
                label="(בס''מ) גובה"
                value={height}
                onChange={(e) => setHeight(e.target.value)}
                onBlur={() => {
                  if (height === '' || (height < 50 || height > 250)) {
                    setHeight('');
                    if (height === '') setHeightError('חובה למלא');
                    else if (height < 50) setHeightError('הגובה לא יכול להיות נמוך מ-50');
                    else setHeightError('הגובה לא יכול להיות גבוה מ-250');
                  } else {
                    setHeightError('');
                  }
                }}
                type="number"
                fullWidth
                margin="normal"
              />
              {heightError && <Typography color="error">{heightError}</Typography>}

              {/* שדה: גיל */}
              <TextField
                label="גיל"
                value={age}
                onChange={(e) => setAge(e.target.value)}
                onBlur={() => {
                  if (age === '' || (age < 10 || age > 120)) {
                    setAge('');
                    if (age === '') setAgeError('חובה למלא');
                    else if (age < 10) setAgeError('ילד מתחת גיל 10 הוא צעיר מדי');
                    else setAgeError('גיל צריך להיות נמוך מ-120');
                  } else {
                    setAgeError('');
                  }
                }}
                type="number"
                fullWidth
                margin="normal"
              />
              {ageError && <Typography color="error">{ageError}</Typography>}

              {/* רמת פעילות */}
              <FormControl fullWidth margin="normal">
                <InputLabel id="activity-level-label">רמת פעילות</InputLabel>
                <Select
                  labelId="activity-level-label"
                  value={activityLevel}
                  onChange={(e) => setActivityLevel(e.target.value)}
                >
                  <MenuItem value="יושבני">יושבני</MenuItem>
                  <MenuItem value="קל">קל</MenuItem>
                  <MenuItem value="בינוני">בינוני</MenuItem>
                  <MenuItem value="פעיל">פעיל</MenuItem>
                  <MenuItem value="מאוד פעיל">פעיל מאוד</MenuItem>
                </Select>
              </FormControl>
              {activityLevelError && <Typography color="error">{activityLevelError}</Typography>}
            </Box>
          )}

          {/* שלב 1 – בחירת מטרה */}
          {activeStep === 1 && (
            <Box>
              <FormLabel>בחר מטרה</FormLabel>
              <RadioGroup
                value={goal}
                onChange={(e) => setGoal(e.target.value)}
                sx={{ textAlign: 'left' }}
              >
                <FormControlLabel value="שמירה" control={<Radio />} label="לשמור על המשקל" />
                <FormControlLabel value="ירידה" control={<Radio />} label="לרדת במשקל" />
                <FormControlLabel value="עליה" control={<Radio />} label="לעלות במשקל" />
              </RadioGroup>
            </Box>
          )}

          {/* כפתורי שליטה: הקודם / הבא */}
          <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
            <Button
              color="inherit"
              disabled={activeStep === 0}
              onClick={handleBack}
              sx={{ mr: 1 }}
            >
              הקודם
            </Button>
            <Box sx={{ flex: '1 1 auto' }} />
            <Button onClick={handleNext}>
              {activeStep === steps.length - 1 ? 'לתפריט' : 'הבא'}
            </Button>
          </Box>
        </React.Fragment>
      )}
    </Box>
  );
}
