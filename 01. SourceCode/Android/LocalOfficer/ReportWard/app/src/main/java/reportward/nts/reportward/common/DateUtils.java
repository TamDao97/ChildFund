package reportward.nts.reportward.common;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.view.View;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TimePicker;

import java.text.SimpleDateFormat;
import java.util.Calendar;

/**
 * Created by thanhld on 3/18/2017.
 */

public class DateUtils {
    /**
     * Định dạng kiểu ngày dd-MM-yyyy
     */
    public static final String DATE_FORMAT_DD_MM_YYYY = "dd/MM/yyyy";
    public static final String DATE_FORMAT_YYYY_MM_DD = "yyyy/MM/dd";

    /**
     * Định dạng kiểu giờ HH:mm
     */
    public static final String DATE_FORMAT_HH_MM = "HH:mm";

    /**
     * Định dạng kiểu thời gian
     */
    public static final String DATE_FORMAT_DD_MM_YYYY_HH_MM = "dd/MM/yyyy HH:mm";

    /**
     * Trả về thời gian hiện tại theo định dạng
     *
     * @param formatDate
     * @return
     */
    public static String CurrentDate(String formatDate) {
        SimpleDateFormat sdf = new SimpleDateFormat(formatDate);
        Calendar now = Calendar.getInstance();
        return sdf.format(now.getTime());
    }

    /**
     * Lấy giá trị ngày hiện tại
     *
     * @return
     */
    public static int CurrentDay() {
        Calendar now = Calendar.getInstance();
        return now.get(Calendar.DAY_OF_MONTH);
    }

    /**
     * Lấy giá trị năm hiện tại
     *
     * @return
     */
    public static int CurrentYear() {
        Calendar now = Calendar.getInstance();
        return now.get(Calendar.YEAR);
    }

    /**
     * Lấy giá tị tháng hiện tại
     *
     * @return
     */
    public static int CurrentMonth() {
        Calendar now = Calendar.getInstance();
        return now.get(Calendar.MONTH);
    }

    /**
     * Lấy giá trị ngày đầu tiên của tháng theo năm
     *
     * @param month
     * @param year
     * @return
     */
    public static String GetFirstDayOfMonth(int month, int year) {
        SimpleDateFormat sdf = new SimpleDateFormat(DATE_FORMAT_DD_MM_YYYY);
        Calendar cal = Calendar.getInstance();
        cal.set(Calendar.YEAR, year);
        cal.set(Calendar.MONTH, month - 1);
        cal.set(Calendar.DAY_OF_MONTH, 1);
        return sdf.format(cal.getTime());
    }

    /**
     * Lấy giá trị ngày cuối cùng của tháng theo năm
     *
     * @param month
     * @param year
     * @return
     */
    public static String GetLastDayOfMonth(int month, int year) {
        SimpleDateFormat sdf = new SimpleDateFormat(DATE_FORMAT_DD_MM_YYYY);
        Calendar cal = Calendar.getInstance();
        cal.set(Calendar.YEAR, year);
        cal.set(Calendar.MONTH, month - 1);
        cal.set(Calendar.DAY_OF_MONTH, cal.getActualMaximum(Calendar.DAY_OF_MONTH));
        return sdf.format(cal.getTime());
    }

    public static String GetDateAddYear(String date, int year) {
        SimpleDateFormat sdf = new SimpleDateFormat(DATE_FORMAT_DD_MM_YYYY);
        Calendar cal = Calendar.getInstance();
        cal.set(Calendar.YEAR, year);
        cal.set(Calendar.MONTH, GetMonthDMY(date));
        cal.set(Calendar.DAY_OF_MONTH, GetDayDMY(date));
        return sdf.format(cal.getTime());
    }

    /**
     * Lấy giá trị đầu tiên của quý theo năm
     *
     * @param quarter
     * @param year
     * @return
     */
    public static String GetFirstDayOfQuarter(int quarter, int year) {
        SimpleDateFormat sdf = new SimpleDateFormat(DATE_FORMAT_DD_MM_YYYY);
        Calendar cal = Calendar.getInstance();
        cal.set(Calendar.YEAR, year);
        cal.set(Calendar.MONTH, (quarter - 1) * 3);
        cal.set(Calendar.DAY_OF_MONTH, cal.getActualMinimum(Calendar.DAY_OF_MONTH));
        return sdf.format(cal.getTime());
    }

    /**
     * Ngày cuối cùng của quý theo năm
     *
     * @param quarter
     * @param year
     * @return
     */
    public static String GetLastDayOfQuarter(int quarter, int year) {
        SimpleDateFormat sdf = new SimpleDateFormat(DATE_FORMAT_DD_MM_YYYY);
        Calendar cal = Calendar.getInstance();
        cal.set(Calendar.YEAR, year);
        cal.set(Calendar.MONTH, quarter * 3 - 1);
        cal.set(Calendar.DAY_OF_MONTH, cal.getActualMaximum(Calendar.DAY_OF_MONTH));
        return sdf.format(cal.getTime());
    }

    /**
     * Lấy giá trị giờ hiện tại
     *
     * @return
     */
    public static int CurrentHour() {
        Calendar now = Calendar.getInstance();
        return now.get(Calendar.HOUR);
    }

    /**
     * Lấy giá trị phút hiện tại
     *
     * @return
     */
    public static int CurrentMinute() {
        Calendar now = Calendar.getInstance();
        return now.get(Calendar.MINUTE);
    }

    /**
     * Lấy giá trị ngày từ kiểu date dd-MM-yyyy
     *
     * @param date
     * @return
     */
    public static int GetDayDMY(String date) {
        return date.isEmpty() ? CurrentDay() : Integer.parseInt(date.substring(0, 2));
    }

    /**
     * Lấy giá trị tháng từ kiểu date dd-MM-yyyy
     *
     * @param date
     * @return
     */
    public static int GetMonthDMY(String date) {
        // Giá trị tháng - 1 để phù hợp với kiểu tháng trong dialog datepicker (0 - 11)
        return date.isEmpty() ? CurrentMonth() : Integer.parseInt(date.substring(3, 5)) - 1;
    }

    /**
     * Lấy giá trị năm từ kiểu date dd-MM-yyyy
     *
     * @param date
     * @return
     */
    public static int GetYearDMY(String date) {
        return date.isEmpty() ? CurrentYear() : Integer.parseInt(date.substring(6, 10));
    }

    /**
     * Lấy giá trị giờ theo thời gian truyền vào
     *
     * @param time
     * @return
     */
    public static int GetHour(String time) {
        return time.isEmpty() ? CurrentHour() : Integer.parseInt(time.substring(0, 2));
    }

    /**
     * Lấy giá trị phút theo thời gian truyền vào
     *
     * @param time
     * @return
     */
    public static int GetMinute(String time) {
        return time.isEmpty() ? CurrentMinute() : Integer.parseInt(time.substring(3, 5));
    }

    /**
     * Chuyển định dạng date kiểu dd/MM/yyyy sang yyyy-MM-dd
     *
     * @param date
     * @return
     */
    public static String ConvertDMYToYMD(String date) {
        if (date == null || date.isEmpty()) {
            return "";
        }
        return date.substring(6, 10) + "-" + date.substring(3, 5) + "-" + date.substring(0, 2);
    }

    /**
     * Chuyển định dạng date từ server về dạng DMY
     *
     * @param date
     * @return
     */
    public static String ConvertYMDServerToDMY(String date) {
        if (date == null || date.isEmpty() || date.equals(Constants.RESULT_STRING_NULL)) {
            return "";
        }
        return date.substring(8, 10) + "/" + date.substring(5, 7) + "/" + date.substring(0, 4);
    }

    /**
     * Chuyển định dạng date từ server về dạng DD/MM/YYYY HH:mm
     *
     * @param date
     * @return
     */
    public static String ConvertYMDServerToDMYHHMM(String date) {
        if (date == null || date.isEmpty() || date.equals(Constants.RESULT_STRING_NULL)) {
            return "";
        }
        return date.substring(8, 10) + "/" + date.substring(5, 7) + "/" + date.substring(0, 4) + " " + date.substring(12, 13) + ":" + date.substring(15, 16);
    }

    /**
     * Chuyển định dạng date từ server về dạng DMY
     *
     * @param time
     * @return
     */
    public static String ConvertHmsServerToHm(String time) {
        if (time == null || time.isEmpty() || time.equals(Constants.RESULT_STRING_NULL)) {
            return "";
        }
        return time.substring(0, 2) + ":" + time.substring(3, 5);
    }

    /**
     * Thêm số giây đằng sau thời gian
     *
     * @param time
     * @return
     */
    public static String AddMilisecondsTime(String time) {
        if (time == null || time.isEmpty()) {
            return "";
        }
        return time + ":00";
    }

    /**
     * Tính số tuổi
     *
     * @return
     */
    public static String GetAge(String date) {
        if (date == null || date.isEmpty()) {
            return "";
        }
        return (CurrentYear() - Integer.parseInt(date.substring(6, 10))) + "";
    }

    /**
     * Gắn hàm hiện dialog datepicker cho button
     *
     * @param context
     * @param btnShowDatePicker
     * @param txtDate
     */
    public static void SetShowDatePicker(final Context context, ImageButton btnShowDatePicker, final EditText txtDate, final Boolean isRequired) {
        btnShowDatePicker.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String date = txtDate.getText().toString();

                DatePickerDialog datePickerDialog = new DatePickerDialog(context, new DatePickerDialog.OnDateSetListener() {
                    @Override
                    public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                        // Giá trị tháng + 1 do giá trị tháng trả về từ datepicker từ 0 - 11
                        monthOfYear += 1;
                        String day = dayOfMonth < 10 ? "0" + dayOfMonth : "" + dayOfMonth;
                        String month = monthOfYear < 10 ? "0" + monthOfYear : "" + monthOfYear;
                        txtDate.setText(day + "/" + month + "/" + year);
                    }
                }, GetYearDMY(date), GetMonthDMY(date), GetDayDMY(date));

                if (!isRequired) {
                    datePickerDialog.setButton(DialogInterface.BUTTON_NEUTRAL, Constants.DIALOG_CONTROL_VALUE_DELETE, new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            txtDate.setText("");
                            dialog.dismiss();
                        }
                    });
                }

                datePickerDialog.show();
            }
        });
    }

    /**
     * Gắn hàm hiện dialog timepicker cho button
     *
     * @param context
     * @param txtTime
     */
    public static void dialogTimePicker(final Context context, final EditText txtTime) {
        String time = txtTime.getText().toString();

        TimePickerDialog timePickerDialog = new TimePickerDialog(context, new TimePickerDialog.OnTimeSetListener() {
            @Override
            public void onTimeSet(TimePicker view, int hourOfDay, int minute) {
                String hour = hourOfDay < 10 ? "0" + hourOfDay : "" + hourOfDay;
                String minutes = minute < 10 ? "0" + minute : "" + minute;
                txtTime.setText(hour + ":" + minutes);
            }
        }, GetHour(time), GetMinute(time), true);

        timePickerDialog.show();
    }

    public static void dialogDatePicker(Context context, final EditText txtDate) {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);
        DatePickerDialog datePickerDialog = new DatePickerDialog(context, new DatePickerDialog.OnDateSetListener() {
            @Override
            public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                // Giá trị tháng + 1 do giá trị tháng trả về từ datepicker từ 0 - 11
                monthOfYear += 1;
                String day = dayOfMonth < 10 ? "0" + dayOfMonth : "" + dayOfMonth;
                String month = monthOfYear < 10 ? "0" + monthOfYear : "" + monthOfYear;
                txtDate.setText(day + "/" + month + "/" + year);
            }
        }, year, month, day);
        datePickerDialog.show();
    }

    /**
     * Chuyển định dạng date từ YYYY-MM-DD HH:mm về dạng DD/MM/YYYY HH:mm
     *
     * @param date
     * @return
     */
    public static String ConvertYMDHHmmServerToDMYHHmm(String date) {
        try {
            if (Utils.isEmpty(date)) {
                return "";
            }
            return date.substring(8, 10) + "/" + date.substring(5, 7) + "/" + date.substring(0, 4) + " " + date.substring(11, 13) + ":" + date.substring(14, 16);
        } catch (Exception ex) {
            return "";
        }
    }

    /**
     * Chuyển định dạng date từ server về dạng DMY
     *
     * @param date
     * @return
     */
    public static String ConvertMDYServerToDMY(String date) {
        try {
            if (date == null || date.isEmpty() || date.equals(Constants.RESULT_STRING_NULL)) {
                return "";
            }
            return date.split(" ")[0];
        }catch (Exception ex){}
        return "";
    }
}
