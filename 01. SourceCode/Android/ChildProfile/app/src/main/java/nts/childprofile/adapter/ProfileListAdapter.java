package nts.childprofile.adapter;


import android.content.Context;
import android.graphics.Color;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.CheckBox;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.androidnetworking.widget.ANImageView;
import com.squareup.picasso.Picasso;

import java.io.File;
import java.util.List;

import nts.childprofile.R;
import nts.childprofile.common.CircularImageView;
import nts.childprofile.common.Constants;
import nts.childprofile.common.DateUtils;
import nts.childprofile.common.GlobalVariable;
import nts.childprofile.model.ChildProfileSearchResult;

public class ProfileListAdapter extends BaseAdapter {

    LayoutInflater layoutInflater;
    static Context context;
    List<ChildProfileSearchResult> listChild;
    Context ctx;
    private OnItemClickListener mOnItemClickListener;
    private boolean isOffline;

    public interface OnItemClickListener {
        void onButtonEditClick(View view, int position, ChildProfileSearchResult obj);

        void onButtonReportClick(View view, int position, ChildProfileSearchResult obj);

        void onSortClick(View view, String columnName);

        void onCaptureClick(View view, String id);

        void onCheckbox(View view, ChildProfileSearchResult obj, boolean isCheck);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }


    public ProfileListAdapter(Context context, List<ChildProfileSearchResult> listChild, boolean isOffline) {
        this.context = context;
        this.listChild = listChild;
        layoutInflater = LayoutInflater.from(context);
        this.isOffline = isOffline;
    }

    @Override
    public int getCount() {
        return listChild.size();
    }

    @Override
    public Object getItem(int position) {
        return listChild.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.item_search_child, null);
            holder = new ViewHolder();
            holder.image = (CircularImageView) convertView.findViewById(R.id.img_ava);
            holder.name = (TextView) convertView.findViewById(R.id.tv_Name);
            holder.DoB = (TextView) convertView.findViewById(R.id.tv_DoB);
            holder.gender = (TextView) convertView.findViewById(R.id.tv_Gender);
            holder.code = (TextView) convertView.findViewById(R.id.tv_Code);
            holder.createDate = (TextView) convertView.findViewById(R.id.tv_DateCreate);
            holder.status = (TextView) convertView.findViewById(R.id.tv_Status);
            holder.address = (TextView) convertView.findViewById(R.id.tv_Address);
            holder.btnReport = (LinearLayout) convertView.findViewById(R.id.btn_Report);
            holder.btnUpdate = (LinearLayout) convertView.findViewById(R.id.btn_Update);
            holder.btnCapture = (LinearLayout) convertView.findViewById(R.id.btnCapture);
            holder.checkboxChildProfile = convertView.findViewById(R.id.checkboxChildProfile);
            holder.imgIsDownloaded = convertView.findViewById(R.id.imgIsDownloaded);

            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        final ChildProfileSearchResult profileModel = listChild.get(position);
        holder.name.setText(profileModel.Name);
        holder.code.setText(profileModel.ChildCode);
        holder.address.setText(profileModel.School);
        if (profileModel.Status.equals("0")) {
            holder.status.setText("Chưa duyệt/Unapproved");
            holder.status.setTextColor(Color.RED);
            holder.createDate.setText(DateUtils.ConvertYMDServerToDMYHHMM(profileModel.CreateDate));
        } else {
            holder.status.setText("Đã duyệt/Approved");
            holder.status.setTextColor(Color.parseColor("#007A45"));
            holder.createDate.setText(DateUtils.ConvertYMDServerToDMYHHMM(profileModel.OfficeApproveDate));
        }
        holder.DoB.setText(DateUtils.ConvertYMDServerToDMY(profileModel.DateOfBirth));
        if (profileModel.Gender.equals("1")) {
            profileModel.Gender = "Nam/Male";
        } else if (profileModel.Gender.equals("0")) {
            profileModel.Gender = "Nữ/Female";
        }
        holder.gender.setText(profileModel.Gender);


        if (profileModel.isDownload) {
            if (!isOffline) {
                holder.imgIsDownloaded.setVisibility(View.VISIBLE);
            }
        } else {
            holder.imgIsDownloaded.setVisibility(View.GONE);
        }

        holder.checkboxChildProfile.setChecked(profileModel.isCheck);

        try {
            if (isOffline) {
                holder.checkboxChildProfile.setVisibility(View.GONE);
                if (profileModel.TypeChildProfile.equals(Constants.TYPE_CHILDPROFILE_SQLITE_CREATED)) {
                    holder.btnReport.setVisibility(View.INVISIBLE);
                    holder.btnCapture.setVisibility(View.INVISIBLE);
                }
            } else {
                holder.checkboxChildProfile.setVisibility(View.VISIBLE);
            }
            if (profileModel.Avata != null && profileModel.Avata != "" && !profileModel.Avata.isEmpty()) {
                if (isOffline) {
                    File file = new File(profileModel.Avata);
                    Uri uri = Uri.fromFile(file);
                    holder.image.setImageURI(uri);

                } else {
                    Picasso.with(this.ctx).load(profileModel.Avata).resize(300, 300)
                            .centerInside().into(holder.image);

                }
            } else {
                Picasso.with(this.ctx).load(R.drawable.ic_people).resize(300, 300)
                        .centerInside().into(holder.image);
            }

        } catch (Exception ex) {
        }

        holder.btnUpdate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onButtonEditClick(view, position, profileModel);
                }
            }
        });

        holder.btnReport.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onButtonReportClick(view, position, profileModel);
                }
            }
        });

        holder.code.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onSortClick(view, "ChildCode");
                }
            }
        });

        holder.name.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onSortClick(view, "Name");
                }
            }
        });

        holder.createDate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onSortClick(view, "CreateDate");
                }
            }
        });

        holder.btnCapture.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onCaptureClick(view, profileModel.Id);
                }
            }
        });

        holder.checkboxChildProfile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final boolean isCheck = ((CheckBox) view).isChecked();
                if (isCheck) {
                    profileModel.isCheck = true;
                } else {
                    profileModel.isCheck = false;
                }
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onCheckbox(view, profileModel, isCheck);
                }
            }
        });

        return convertView;
    }
}
