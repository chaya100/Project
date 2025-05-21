import React, { useEffect, useState, useRef } from 'react'; // ייבוא React ופונקציות נדרשות
import { Card, CardContent, Typography, Grid, Fade } from '@mui/material'; // רכיבי UI מ-MUI
import axios from 'axios'; // ייבוא axios לשליפת נתונים מ-API
import FacebookCircularProgress from './LoadingSpinner'; // הספינר שהכנת לקוד

// נתוני הארוחות, נתונים קבועים למידע כללי
const meals = [
    { id: 1, name: 'ארוחת בוקר' },
    { id: 2, name: 'ארוחת צהריים' },
    { id: 4, name: 'ארוחת ביניים' },
    { id: 3, name: 'ארוחת ערב' },
];

export default function MealsDisplay(props) {
    const [caloriesData, setCaloriesData] = useState([]); // סטייט לאחסון נתוני הקלוריות
    const [loading, setLoading] = useState(true); // סטייט לשליטה בטעינה (לצורך אנימציה)
    const fetched = useRef(false); // משתנה לשמירת סטטוס של טעינה (למנוע טעינה כפולה)

    // פונקציה לחישוב קלוריות ע"י שליפה מ-API
    const calculateCalories = async (age, weight, height, isman, lifestyle, purpose) => {
        try {
            setLoading(true); // התחלת טעינה
            const response = await axios.get(`https://localhost:7227/api/User/CalculateSimplex/${age}/${weight}/${height}/${isman}/${lifestyle}/${purpose}`);
            setCaloriesData(response.data); // עדכון הנתונים עם תוצאת ה-API
        } catch (error) {
            console.error("Error fetching data:", error); // טיפול בשגיאות
        } finally {
            setLoading(false); // סיום טעינה
        }
    };

    // useEffect עבור טעינת הנתונים רק פעם אחת לאחר שינוי props
    useEffect(() => {
        if (!fetched.current) {
            calculateCalories(props.age, props.weight, props.height, props.isman, props.lifestyle, props.purpose); // קריאה לפונקציה שמבצעת את החישוב
            fetched.current = true; // עדכון סטטוס טעינה
        }
    }, [props]); // הפונקציה תרוץ בכל פעם שה-props משתנים

    return (
        <div style={{ padding: '20px' }}>
            {/* שכבת טעינה עם אנימציה, מציג את הספינר */}
            <Fade in={loading} timeout={500} unmountOnExit>
                <div style={{
                    position: 'fixed',
                    top: 0,
                    right: 0,
                    bottom: 0,
                    left: 0,
                    backgroundColor: 'rgba(0, 0, 0, 0.4)', // רקע כהה למחצה
                    zIndex: 1300,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    flexDirection: 'column',
                }}>
                    <FacebookCircularProgress /> {/* רכיב ספינר מותאם אישית */}
                    <Typography variant="h6" sx={{ marginTop: 2, color: 'white' }}>
                        טוען את התפריט שלך {/* טקסט המודיע על טעינה */}
                    </Typography>
                </div>
            </Fade>

            {/* כותרת מרכזית */}
            <Typography variant="h4" gutterBottom align="center">
                התפריט שלך
            </Typography>

            {/* Grid המציג את כל הארוחות */}
            <Grid container spacing={2} direction="row-reverse">
                {meals.map((meal) => (
                    <Grid item xs={12} sm={6} md={3} key={meal.id}>
                        <Card>
                            <CardContent>
                                <Typography variant="h5" align="center">{meal.name}</Typography> {/* שם הארוחה */}
                                {/* הצגת מאכלים עבור כל ארוחה */}
                                {caloriesData.filter(item => item.mealNumber === meal.id).length > 0 ? (
                                    caloriesData.filter(item => item.mealNumber === meal.id).map((foodItem, index) => (
                                        <div key={index} style={{ textAlign: 'center' }}>
                                            {foodItem.food} {/* שם המאכל */}
                                        </div>
                                    ))
                                ) : (
                                    <div style={{ textAlign: 'center' }}>אין מאכלים זמינים</div> /* הודעה אם אין מאכלים זמינים */
                                )}
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </div>
    );
}
