package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.PrisonerModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class PrisonerDetailListAdapter extends RecyclerView.Adapter<PrisonerDetailListAdapter.ViewHolder> {

    private final int mBackground;
    private List<PrisonerModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onEditClick(View view, int position, PrisonerModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lyName, lyGender, lyBirthday, lyAge, lyPhone, lyAddress, lyRelationship;
        public TextView txtName, txtGender, txtBirthday, txtAge, txtPhone, txtAddress, txtRelationship;
        public RecyclerView rvAbuse;
        public TextView txtEdit, txtTitle;

        public ViewHolder(View v) {
            super(v);
            lyName = v.findViewById(R.id.lyName);
            lyGender = v.findViewById(R.id.lyGender);
            lyBirthday = v.findViewById(R.id.lyBirthday);
            lyAge = v.findViewById(R.id.lyAge);
            lyPhone = v.findViewById(R.id.lyPhone);
            lyAddress = v.findViewById(R.id.lyAddress);
            lyRelationship = v.findViewById(R.id.lyRelationship);

            txtName = v.findViewById(R.id.txtName);
            txtGender = v.findViewById(R.id.txtGender);
            txtBirthday = v.findViewById(R.id.txtBirthday);
            txtAge = v.findViewById(R.id.txtAge);
            txtPhone = v.findViewById(R.id.txtPhone);
            txtRelationship = v.findViewById(R.id.txtRelationship);
            //txtAddress = v.findViewById(R.id.txtAddress);

            rvAbuse = v.findViewById(R.id.rvAbuse);

            txtEdit = v.findViewById(R.id.txtEdit);
            txtTitle = v.findViewById(R.id.txtTitle);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public PrisonerDetailListAdapter(Context ctx, List<PrisonerModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<PrisonerModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public PrisonerDetailListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_prisoner_detail, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final PrisonerModel prisonerModel = listObject.get(position);

        holder.txtTitle.setText(holder.txtTitle.getText() + " #" + (position + 1));

        if (!Utils.isEmpty(prisonerModel.Name)) {
            holder.lyName.setVisibility(View.VISIBLE);
            holder.txtName.setText(prisonerModel.Name);
        } else {
            holder.lyName.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.GenderName)) {
            holder.lyGender.setVisibility(View.VISIBLE);
            holder.txtGender.setText(prisonerModel.GenderName);
        } else {
            holder.lyGender.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.Birthday)) {
            holder.lyBirthday.setVisibility(View.VISIBLE);
            holder.txtBirthday.setText(DateUtils.ConvertYMDServerToDMY(prisonerModel.Birthday));
        } else {
            holder.lyBirthday.setVisibility(View.GONE);
        }

        if (prisonerModel.Age != null) {
            holder.lyAge.setVisibility(View.VISIBLE);
            holder.txtAge.setText(String.valueOf(prisonerModel.Age));
        } else {
            holder.lyAge.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.Phone)) {
            holder.lyPhone.setVisibility(View.VISIBLE);
            holder.txtPhone.setText(String.valueOf(prisonerModel.Phone));
        } else {
            holder.lyPhone.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.RelationshipName)) {
            holder.lyRelationship.setVisibility(View.VISIBLE);
            holder.txtRelationship.setText(String.valueOf(prisonerModel.RelationshipName));
        } else {
            holder.lyRelationship.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.FullAddress)) {
            holder.lyAddress.setVisibility(View.VISIBLE);
            holder.txtAddress.setText(String.valueOf(prisonerModel.FullAddress));
        } else {
            holder.lyAddress.setVisibility(View.GONE);
        }

        holder.txtEdit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onEditClick(view, position, prisonerModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
