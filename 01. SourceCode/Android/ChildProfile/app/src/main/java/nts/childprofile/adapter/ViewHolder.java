package nts.childprofile.adapter;

import android.support.v7.widget.AppCompatCheckBox;
import android.widget.CheckBox;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.androidnetworking.widget.ANImageView;

import nts.childprofile.common.CircularImageView;

public class ViewHolder {
    TextView name;
    TextView code;
    TextView DoB;
    TextView gender;
    TextView address;
    TextView status;
    TextView createDate;
    CircularImageView image;
    LinearLayout btnReport;
    LinearLayout btnUpdate;
    LinearLayout btnCapture;
    AppCompatCheckBox checkboxChildProfile;
    ImageView imgIsDownloaded;
}
