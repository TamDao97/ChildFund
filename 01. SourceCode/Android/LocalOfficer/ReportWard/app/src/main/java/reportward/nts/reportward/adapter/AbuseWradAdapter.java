package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.model.ReportByAbuseWardModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class AbuseWradAdapter extends RecyclerView.Adapter<AbuseWradAdapter.ViewHolder> {

    private final int mBackground;
    private List<ReportByAbuseWardModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtYear, txtTotal;

        public ViewHolder(View v) {
            super(v);
            txtYear = (TextView) v.findViewById(R.id.txtYear);
            txtTotal = (TextView) v.findViewById(R.id.txtTotal);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public AbuseWradAdapter(Context ctx, List<ReportByAbuseWardModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public AbuseWradAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_count_wrad, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ReportByAbuseWardModel reportByCountWardModel = filtered_items.get(position);
        holder.txtYear.setText(String.valueOf(reportByCountWardModel.AbuseName));
        holder.txtTotal.setText(String.valueOf(reportByCountWardModel.Count));
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return filtered_items.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
